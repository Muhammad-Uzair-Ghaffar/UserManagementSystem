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
        Task<ServiceResponse<List<GetUserDto>>> GetAllUsers();
         Task<ServiceResponse<GetUserDto>> GetUserById(int id);
         Task<ServiceResponse<List<User>>> AddUser(AddUserDto newUser);
         Task<ServiceResponse<List<User>>> DeleteUser(int id);
         Task<ServiceResponse<List<User>>> UpdateUser(int id,User newUser);
    }
}