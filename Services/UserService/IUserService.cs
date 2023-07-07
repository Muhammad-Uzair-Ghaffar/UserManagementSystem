using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagementSystem.Models;

namespace UserManagementSystem.Services.UserService
{
    public interface IUserService
    {
        Task<ServiceResponse<List<User>>> GetAllUsers();
         Task<ServiceResponse<User>> GetUserById(int id);
         Task<ServiceResponse<List<User>>> AddUser(User newUser);
         Task<ServiceResponse<List<User>>> DeleteUser(int id);
         Task<ServiceResponse<List<User>>> UpdateUser(int id,User newUser);
    }
}