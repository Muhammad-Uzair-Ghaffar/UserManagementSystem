
using Microsoft.AspNetCore.Mvc;
using UserManagementSystem.Models;
using System.Collections.Generic;
using UserManagementSystem.Services.UserService;
using UserManagementSystem.Dtos.User;

namespace UserManagementSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController: ControllerBase
    {
        
        private readonly IUserService _userService;
        
        public UserController(IUserService userService )
        {
            _userService = userService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult>  Get()
        {
            return Ok(await _userService.GetAllUsers());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult>  GetSingle(int id)
        {
            return Ok(await _userService.GetUserById(id));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id){
           
            return Ok(await _userService.DeleteUser(id));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult>  UpdateUser(User newUser,int id){
            
            return Ok(await _userService.UpdateUser( id , newUser ));
        }

        [HttpPost]
        public async Task<IActionResult>  AddUser(User newUser){
            
            return Ok(await _userService.AddUser(newUser));
        }
    }
}