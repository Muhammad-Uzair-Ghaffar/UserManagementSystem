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
         Task<User> GetUserById(string id);
         Task<List<User>> AddUser(User newUser);
         Task<User> DeleteUser(string id);
         Task<User> UpdateUser(string id,User newUser);
        Task<User>  Register(User newUser, string password);
        Task<string> Login(string useremail, string password);
        Task<bool> UserExists(string useremail);
    }
}