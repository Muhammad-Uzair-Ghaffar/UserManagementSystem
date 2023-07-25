using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserManagementSystem.Dtos.Role;
using UserManagementSystem.Dtos.User;
using UserManagementSystem.Models;
using UserManagementSystem.Services.RoleService;
using UserManagementSystem.Services.UserService;

namespace UserManagementSystem.Controllers
{
    [Route("api/v1/[controller]")]
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;
        public RoleController(IRoleService roleService, IMapper mapper)
        {
            _roleService = roleService;
            _mapper = mapper;
        }

        [HttpDelete("DeleteUserRole")]
        public async Task<IActionResult> DeleteUserRole([FromBody] UserRoleDto userrole)
        {

            try
            {
                await _roleService.DeleteUserRole(userrole.UserId,userrole.RoleId);
                return Ok(null, "Role with given Id have been deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(null, ex.Message);
            }

        }


        [HttpDelete("DeleteRole/{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {

            try
            {
                await _roleService.DeleteRole(id);
                return Ok(null, "Role with given Id have been deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(null, ex.Message);
            }

        }


        [HttpPost("AddUserRole")]
        public async Task<IActionResult> AddUserRole([FromBody] UserRoleDto newUserRole)
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
                    IdentityUserRole<string> userrole = _mapper.Map<IdentityUserRole<string>>(newUserRole);
                    IdentityUserRole<string> newuserrole = await _roleService.AddUserRole(userrole);
                    return Ok(newuserrole, "A new role realation have been added successfully");

                }
                catch (Exception ex)
                {
                    return BadRequest(null, ex.Message);
                }

            }
        }
        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRole([FromBody] RoleDto newRole)
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
                    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    IdentityRole role = _mapper.Map<IdentityRole>(newRole);
                    role.Id = Guid.NewGuid().ToString();
                    List<IdentityRole> users = await _roleService.AddRole(role,userId);
                    return Ok((users.Select(c => _mapper.Map<RoleDto>(c)).ToList()), "A new role have been added successfully");

                }
                catch (Exception ex)
                {
                    return BadRequest(null, ex.Message);
                }

            }
        }
        [HttpGet("GetAllUserRoles")]
        public async Task<IActionResult> GetAllUserRoles()
        {
            try
            {
                List<IdentityUserRole<string>> userroles = await _roleService.GetAllUserRoles();
                return Ok((userroles.Select(c => _mapper.Map<UserRoleDto>(c)).ToList()), "This is the list of all relations");
            }
            catch (Exception ex)
            {
                return BadRequest(null, ex.Message);
            }
        }
        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                List<IdentityRole> roles = await _roleService.GetAllRoles();
                return Ok((roles.Select(c => _mapper.Map<RoleDto>(c)).ToList()), "This is the list of all roles");
            }
            catch (Exception ex)
            {
                return BadRequest(null, ex.Message);
            }
        }
    }
}
