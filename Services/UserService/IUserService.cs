using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagementSystem.Models;
using UserManagementSystem.Dtos.User;
using Microsoft.AspNetCore.Identity;

namespace UserManagementSystem.Services.UserService
{
    public interface IUserService
    {
        Task<List<IdentityUser>> GetAllUsers();
        Task<List<IdentityUser>> GetUsersWithPagination(int page, int pageSize, string searchBy, string filter, string sortBy, bool ascending);
        Task<IdentityUser> GetUserById(string id);
        // Task<List<IdentityUser>> AddUser(IdentityUser newUser);
         Task<IdentityUser> DeleteUser(string id);
       //  Task<IdentityUser> UpdateUser(string id, IdentityUser newUser);
    }
}