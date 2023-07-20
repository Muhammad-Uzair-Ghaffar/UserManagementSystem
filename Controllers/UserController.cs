
using Microsoft.AspNetCore.Mvc;
using UserManagementSystem.Models;
using System.Collections.Generic;
using UserManagementSystem.Services.UserService;
using UserManagementSystem.Dtos.User;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace UserManagementSystem.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    public class UserController : BaseController
    {

        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> Get()
        {
            try
            {
                List<IdentityUser> users = await _userService.GetAllUsers();
                return Ok((users.Select(c => _mapper.Map<UserDto>(c)).ToList()), "This is the list of all users");
            }
            catch (Exception ex)
            {
                return BadRequest(null, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingle(string id)
        {
            try
            {
                IdentityUser user = await _userService.GetUserById(id);
                return Ok(_mapper.Map<UserDto>(user), "This is the user with given ID");

            }
            catch (Exception ex)
            {
                return BadRequest(null, ex.Message);
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {

            try
            {
                await _userService.DeleteUser(id);
                return Ok(null, "User with given Id have been deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(null, ex.Message);
            }

        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateUser([FromBody] UserDto newUser, string id)
        //{

        //    if (!ModelState.IsValid)
        //    {
        //        List<string> errors = ModelState.Values.SelectMany((v) => v.Errors).Select(e => e.ErrorMessage).ToList();
        //        return BadRequest(null, errors[0]);
        //    }
        //    else
        //    {
        //        try
        //        {
        //            IdentityUser user = _mapper.Map<IdentityUser>(newUser);
        //            IdentityUser updateduser = await _userService.UpdateUser(id, user);
        //            return Ok(updateduser, "User have been updated successfully");
        //        }
        //        catch (Exception ex)
        //        {
        //            return BadRequest(null, ex.Message);
        //        }
        //    }
        //}

        

       

    }
}