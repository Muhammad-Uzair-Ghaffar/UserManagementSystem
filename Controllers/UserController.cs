
using Microsoft.AspNetCore.Mvc;
using UserManagementSystem.Models;
using System.Collections.Generic;
using UserManagementSystem.Services.UserService;
using UserManagementSystem.Dtos.User;
using AutoMapper;

namespace UserManagementSystem.Controllers
{
   
    [Route("[controller]")]
    public class UserController: ControllerBase
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
            ServiceResponse<List<GetUserDto>> serviceResponse = new ServiceResponse<List<GetUserDto>>();
            List<User> users=await _userService.GetAllUsers();
            serviceResponse.Data= (users.Select(c=> _mapper.Map<GetUserDto>(c)).ToList());
            return Ok(serviceResponse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult>  GetSingle(Guid id)
        {
            ServiceResponse<GetUserDto> serviceResponse = new ServiceResponse<GetUserDto>();
            User user = await _userService.GetUserById(id);
            serviceResponse.Data = _mapper.Map<GetUserDto>(user);
            return Ok(serviceResponse);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            ServiceResponse<List<GetUserDto>> serviceResponse = new ServiceResponse<List<GetUserDto>>();
            List<User> users = await _userService.DeleteUser(id);
            serviceResponse.Data = (users.Select(c => _mapper.Map<GetUserDto>(c)).ToList());
            return Ok(serviceResponse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult>  UpdateUser([FromBody]  AddUserDto newUser,Guid id)
        {
            ServiceResponse<List<GetUserDto>> serviceResponse = new ServiceResponse<List<GetUserDto>>();
            if (!ModelState.IsValid)
            {
                List<string> errors = ModelState.Values.SelectMany((v)=> v.Errors).Select(e => e.ErrorMessage).ToList();
                serviceResponse.Message = errors[0];
                serviceResponse.status = Status.BadRequest;
                return BadRequest(serviceResponse);

            }
            else
            {
                try
                {
                    User user = _mapper.Map<User>(newUser);
                    List<User> users = await _userService.UpdateUser(id, user);
                    serviceResponse.Data = (users.Select(c => _mapper.Map<GetUserDto>(c)).ToList());
                    return Ok(serviceResponse);
                }
                catch ( Exception ex)
                {
                    serviceResponse.Message = ex.Message;
                    serviceResponse.status = Status.BadRequest;
                    return BadRequest(serviceResponse);
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult>  AddUser([FromBody] AddUserDto newUser)
        {
            ServiceResponse<List<GetUserDto>> serviceResponse = new ServiceResponse<List<GetUserDto>>();
            if (!ModelState.IsValid)
            {
                serviceResponse.Message = "Insufficient credentials to proceed with request ";
                serviceResponse.status = Status.BadRequest;
                return BadRequest(serviceResponse);

            }
            else
            {
                try
                {
                    User user = _mapper.Map<User>(newUser);
                    List<User> users = await _userService.AddUser(user);
                    serviceResponse.Data = (users.Select(c => _mapper.Map<GetUserDto>(c)).ToList());
                    return Ok(serviceResponse);

                }
                catch (Exception ex)
                {
                    serviceResponse.Message = ex.Message;
                    serviceResponse.status = Status.BadRequest;
                    return BadRequest(serviceResponse);
                }
                
            }
        }
    }
}