using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagementSystem.Models;
using UserManagementSystem.Dtos.User;
using Microsoft.AspNetCore.Identity;

namespace UserManagementSystem.Services.RoleService
{
    public interface IRoleService
    {
        Task<List<IdentityRole>> GetAllRoles();
        Task<List<IdentityUserRole<string>>> GetAllUserRoles();
        Task<List<IdentityRole>> AddRole(IdentityRole newRole,string userId);
        Task<IdentityUserRole<string>> AddUserRole(IdentityUserRole<string> newUserrole);
        Task DeleteRole(string id);
        Task DeleteUserRole(string userId, string roleId);
        Task IsAdmin(string userId);

    }
}