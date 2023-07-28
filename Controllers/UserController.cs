
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
    [Authorize(AuthenticationSchemes =("Bearer"))]
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
      //  [Authorize( Roles ="Admin")]
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

        [AllowAnonymous]
        [HttpGet("GetAllPagination")]
        public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string searchBy = "", [FromQuery] string sortBy = "")
        {
            try
            {
                var users = await _userService.GetUsersWithPagination(page, pageSize, searchBy, sortBy);
                return Ok((users.Select(c => _mapper.Map<UserDto>(c)).ToList()), "This is the list of all users");
            }
            catch (Exception ex)
            {
                return BadRequest(null, ex.Message);
            }
        }

    }
}