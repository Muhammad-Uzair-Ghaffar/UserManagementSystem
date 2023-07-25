using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
using UserManagementSystem.GenericRepository;
using UserManagementSystem.Models;

namespace UserManagementSystem.Services.RoleService
{
    public class RoleService : IRoleService
    {


        private readonly IRepository<IdentityRole> _roleRepository;
        private readonly IRepository<IdentityUserRole<string>> _userRoleRepository;
        private readonly string _userId;
        public RoleService(IRepository<IdentityRole> roleRepository, IRepository<IdentityUserRole<string>> userRoleRepository, IHttpContextAccessor httpContextAccessor)
        {
           
            _userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            _roleRepository = roleRepository;

            _userRoleRepository = userRoleRepository;
        }

        public async Task<List<IdentityRole>> AddRole(IdentityRole newRole, string userId)
        {
           
           
           // await IsAdmin(_userId);
            if (newRole.Name == "")
            {
                throw new Exception("Role Name cannot be empty");
            }
            bool roleExists = _roleRepository.GetQueryable().Any(c => c.Name == newRole.Name);
            if (roleExists)
            {
                throw new Exception("Role already exists");
            }
            _roleRepository.Insert(newRole);
            await _roleRepository.SaveChangesAsync();

            return await _roleRepository.GetAllAsync();
        }


       

        public async Task<IdentityUserRole<string>> AddUserRole(IdentityUserRole<string> newUserrole)
        {
           // await IsAdmin(_userId);
            bool relationExists = await _userRoleRepository.GetQueryable().AnyAsync(c => c.UserId == newUserrole.UserId && c.RoleId == newUserrole.RoleId);
            if (relationExists)
            {
                throw new Exception("Relation of user with provided role already exists");
            }
            _userRoleRepository.Insert( newUserrole);
            await _userRoleRepository.SaveChangesAsync();
            return newUserrole;
        }

        public async Task DeleteRole(string id)
        {

            // await IsAdmin(_userId);
            IdentityRole role = await _roleRepository.GetById(id);
            if (role == null)
            {
                throw new Exception("User does not exist");
            }
            _roleRepository.Delete(role);
            await _roleRepository.SaveChangesAsync();
            
        }

        public async Task DeleteUserRole(string userId, string roleId)
        {
            //await IsAdmin(_userId);
            IdentityUserRole<string> userRole = await _userRoleRepository.GetQueryable().FirstAsync(c => c.UserId == userId && c.RoleId == roleId);
            _userRoleRepository.Delete(userRole);
            await _userRoleRepository.SaveChangesAsync();
        }

        public async Task<List<IdentityUserRole<string>>> GetAllUserRoles()
        {
           // await IsAdmin(_userId);
            List< IdentityUserRole<string> > userroles = await _userRoleRepository.GetAllAsync(); 
            return userroles;
        }

        public async Task<List<IdentityRole>> GetAllRoles()
        {
            //await IsAdmin(_userId);
            List<IdentityRole> roles = await _roleRepository.GetAllAsync();
            return roles;
        }

        public async Task IsAdmin(string userId)
        {
            IdentityRole adminRole = await _roleRepository.GetQueryable().FirstOrDefaultAsync(c => c.Name == "Admin");

            if (adminRole != null)
            {
                string adminRoleId = adminRole.Id;
                bool isAdmin = _roleRepository.GetQueryable().Any(c => c.Id == userId && c.Name == adminRoleId);
                if (!isAdmin)
                {
                    throw new Exception("User is unauthorized");
                }

            }
            else
            {
                throw new Exception("User is unauthorized");
            }
            
        }
    }
}
