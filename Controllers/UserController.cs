
using Microsoft.AspNetCore.Mvc;
using UserManagementSystem.Models;
using System.Collections.Generic;
using UserManagementSystem.Services.UserService;
using UserManagementSystem.Dtos.User;
using AutoMapper;

namespace UserManagementSystem.Controllers
{
   
    [Route("[controller]")]
    public class UserController: BaseController
    {
        
        private readonly IUserService _userService;
       private readonly IMapper _mapper;
        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult>  Get()
        {
            List<User> users=await _userService.GetAllUsers();
            return Ok((users.Select(c => _mapper.Map<GetUserDto>(c)).ToList()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult>  GetSingle(string id)
        {
            try { 
            User user = await _userService.GetUserById(id);
            return Ok(_mapper.Map<GetUserDto>(user));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            
                try
                {
                    List<User> users = await _userService.DeleteUser(id);
                    return Ok((users.Select(c => _mapper.Map<GetUserDto>(c)).ToList()));
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
         
        }

        [HttpPut("{id}")]
        public async Task<IActionResult>  UpdateUser([FromBody]  AddUserDto newUser,string id)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            if (!ModelState.IsValid)
            {
                List<string> errors = ModelState.Values.SelectMany((v)=> v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest("", errors[0]);

            }
            else
            {
                try
                {
                    User user = _mapper.Map<User>(newUser);
                    List<User> users = await _userService.UpdateUser(id, user);
                    return Ok((users.Select(c => _mapper.Map<GetUserDto>(c)).ToList()));
                }
                catch ( Exception ex)
                {
                    return BadRequest(ex);
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult>  AddUser([FromBody] AddUserDto newUser)
        {
            
            if (!ModelState.IsValid)
            {
                List<string> errors = ModelState.Values.SelectMany((v) => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest("", errors[0]);

            }
            else
            {
                try
                {
                    User user = _mapper.Map<User>(newUser);
                    user.Id= Guid.NewGuid().ToString();
                    List<User> users = await _userService.AddUser(user);
                    return Ok((users.Select(c => _mapper.Map<GetUserDto>(c)).ToList()));

                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }
                
            }
        }
    }
}