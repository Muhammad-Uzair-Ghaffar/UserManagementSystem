using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagementSystem.Models;
using UserManagementSystem.Dtos.User;

namespace UserManagementSystem.Services.UserService
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsers();
         Task<User> GetUserById(Guid id);
         Task<List<User>> AddUser(User newUser);
         Task<List<User>> DeleteUser(Guid id);
         Task<List<User>> UpdateUser(Guid id,User newUser);
    }
}