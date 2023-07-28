using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagementSystem.Dtos.Role;
using UserManagementSystem.Dtos.User;
using UserManagementSystem.Models;

namespace UserManagementSystem.Controllers
{
    //routes made 
    //add new role
    //get al roles
    //add userrole
    //delete role 
    //delete user role

    [Authorize(AuthenticationSchemes = ("Bearer"))]
    [Route("api/v1/[controller]")]
    public class AdminController : BaseController
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(RoleManager<IdentityRole> roleManager, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _mapper = mapper;
            _userManager = userManager;
        }
        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRole([FromBody] RoleDto Rolename)
        {
            if (!ModelState.IsValid)
            {
                List<string> errors = ModelState.Values.SelectMany((v) => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(null, errors[0]);
            }
            else
            {
                try
                {
                    var role = new IdentityRole
                    {
                        Name = Rolename.Name
                    };
                    var result = await _roleManager.CreateAsync(role);

                    if (result.Succeeded)
                    {
                        RoleDto roleDto = _mapper.Map<RoleDto>(role);
                        return Ok(roleDto, "New Role is succeccfully added");

                    }
                    var errorDescriptions = result.Errors.Select(error => error.Description).ToList();
                    return BadRequest(errorDescriptions);
                }
                catch (Exception ex)
                {
                    return BadRequest(null, ex.Message);
                }
            }
        }
        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                var roles = await _roleManager.Roles.ToListAsync();
                List<RoleDto> roleDto = _mapper.Map<List<RoleDto>>(roles);
                return Ok(roleDto,"This is the list of all the roles");
            }
            catch (Exception ex)
            {
                return BadRequest(null, ex.Message);
            }
        }
        [HttpPost("AddUserRole")]
        public async Task<IActionResult> AddUserRole([FromBody] UserRoleDto userRoleDto)
        {
            if (!ModelState.IsValid)
            {
                List<string> errors = ModelState.Values.SelectMany((v) => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }
            else
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(userRoleDto.UserId);
                    if (user == null)
                    {
                        return Ok(null, $"User with ID {userRoleDto.UserId} not found.");
                    }

                    var role = await _roleManager.FindByNameAsync(userRoleDto.RoleName);
                    if (role == null)
                    {
                        return Ok(null, $"Role with name {userRoleDto.RoleName} not found.");
                    }

                    var result = await _userManager.AddToRoleAsync(user, role.Name);
                    if (result.Succeeded)
                    {
                        return Ok(null, $"User with ID {userRoleDto.UserId} added to role {userRoleDto.RoleName} successfully.");
                    }
                    else
                    {
                        var errorDescriptions = result.Errors.Select(error => error.Description).ToList();
                        return BadRequest(errorDescriptions);
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(null, ex.Message);
                }
            }
        }
        [HttpDelete("DeleteUserRole")]
        public async Task<IActionResult> DeleteUserRole([FromBody] UserRoleDto userRoleDto)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userRoleDto.UserId);
                if (user == null)
                {
                    return Ok(null, $"User with ID {userRoleDto.UserId} not found.");
                }

                var role = await _roleManager.FindByNameAsync(userRoleDto.RoleName);
                if (role == null)
                {
                    return Ok(null, $"Role with name {userRoleDto.RoleName} not found.");
                }

                var result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                if (result.Succeeded)
                {
                    return Ok(null, $"User with ID {userRoleDto.UserId} removed from role {userRoleDto.RoleName} successfully.");
                }
                else
                {
                    var errorDescriptions = result.Errors.Select(error => error.Description).ToList();
                    return BadRequest(errorDescriptions);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(null,ex.Message);
            }
        }
        [HttpDelete("DeleteRole")]
        public async Task<IActionResult> DeleteRole([FromBody] RoleDto roleDto)
        {
            try
            {
                var role = await _roleManager.FindByNameAsync(roleDto.Name);
                if (role != null)
                {
                    var result = await _roleManager.DeleteAsync(role);
                    if (result.Succeeded)
                    {
                        return Ok(null, $"Role with name {roleDto.Name} have been deleted successfully.");
                    }
                    else
                    {
                        var errorDescriptions = result.Errors.Select(error => error.Description).ToList();
                        return BadRequest(errorDescriptions);
                    }

                }
                else
                {
                    return Ok(null, $"Role with name {roleDto.Name} not found.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(null,ex.Message);
            }
        }

    }
}
