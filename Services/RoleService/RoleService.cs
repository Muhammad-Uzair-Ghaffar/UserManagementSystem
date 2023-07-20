using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
using UserManagementSystem.Data;
using UserManagementSystem.Models;

namespace UserManagementSystem.Services.RoleService
{
    public class RoleService : IRoleService
    {

        private readonly DataContext _context;
        private readonly string _userId;
        public RoleService(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public async Task<List<Role>> AddRole(Role newRole, string userId)
        {
           
           
            await IsAdmin(_userId);
            if (newRole.Name == "")
            {
                throw new Exception("Role Name cannot be empty");
            }
            bool roleExists = _context.Roles.Any(c => c.Name == newRole.Name);
            if (roleExists)
            {
                throw new Exception("Role already exists");
            }
            await _context.Roles.AddAsync(newRole);
            await _context.SaveChangesAsync();

            return await _context.Roles.ToListAsync();
        }


       

        public async Task<UserRole> AddUserRole(UserRole newUserrole)
        {
            await IsAdmin(_userId);
            bool relationExists = await _context.UserRoles.AnyAsync(c => c.UserId == newUserrole.UserId && c.RoleId == newUserrole.RoleId);
            if (relationExists)
            {
                throw new Exception("Relation of user with provided role already exists");
            }
             await _context.UserRoles.AddAsync(newUserrole);
            await _context.SaveChangesAsync();
            UserRole relation = await _context.UserRoles.FirstAsync(c => c.UserId == newUserrole.UserId);
            return relation;
        }

        public async Task DeleteRole(string id)
        {
            await IsAdmin(_userId);
            Role role = await _context.Roles.FirstAsync(c => c.Id == id);
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserRole(string userId, string roleId)
        {
            await IsAdmin(_userId);
            UserRole userrole = await _context.UserRoles.FirstAsync(c => c.UserId == userId && c.RoleId == roleId);
            _context.UserRoles.Remove(userrole);
            await _context.SaveChangesAsync();
        }

        public async Task<List<UserRole>> GetAllUserRoles()
        {
            await IsAdmin(_userId);
            List<UserRole> userroles = await _context.UserRoles.ToListAsync();
            return userroles;
        }

        public async Task<List<Role>> GetAllRoles()
        {
            await IsAdmin(_userId);
            List<Role> roles = await _context.Roles.ToListAsync();
            return roles;
        }

        public async Task IsAdmin(string userId)
        {
            Role adminRole = await _context.Roles.FirstOrDefaultAsync(c => c.Name == "Admin");

            if (adminRole != null)
            {
                string adminRoleId = adminRole.Id;
                bool isAdmin = _context.UserRoles.Any(c => c.UserId == userId && c.RoleId == adminRoleId);
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
