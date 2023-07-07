using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagementSystem.Dtos.User;
using UserManagementSystem.Models;

namespace UserManagementSystem.Services.UserService
{
    
    public class UserService : IUserService
    {
        private static List<User> users = new List<User>
        {
            new User(),
            new User { Id = 1,Name = "saim"}
        }; 

        public async Task<ServiceResponse<List<GetUserDto>>> AddUser(AddUserDto newUser)
        {
            ServiceResponse<List<GetUserDto>> serviceResponse = new ServiceResponse<List<GetUserDto>>();
            users.Add(newUser);
            serviceResponse.Data = users;
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetUserDto>>>  DeleteUser(int id)
        {
            ServiceResponse<List<GetUserDto>> serviceResponse = new ServiceResponse<List<GetUserDto>>();
            User user = users.First(c=> c.Id == id);
            users.Remove(user);
            serviceResponse.Data = users;
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetUserDto>>>  GetAllUsers()
        {
            ServiceResponse<List<GetUserDto>> serviceResponse = new ServiceResponse<List<GetUserDto>>();
            serviceResponse.Data = users;
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetUserDto>> GetUserById(int id)
        {
            ServiceResponse<GetUserDto> serviceResponse = new ServiceResponse<GetUserDto>();
            serviceResponse.Data = users.FirstOrDefault(c=> c.Id == id);
            return serviceResponse;
            
        }

        public async Task<ServiceResponse<List<GetUserDto>>>  UpdateUser(int id, User newUser)
        {
            ServiceResponse<List<GetUserDto>> serviceResponse = new ServiceResponse<List<GetUserDto>>();
            User user = users.FirstOrDefault(c=> c.Id == id);
            user.Name=newUser.Name;
            user.email=newUser.email;
            user.Class=newUser.Class;
            Console.WriteLine(newUser.Name);
            serviceResponse.Data=users;
            return serviceResponse;
        }

       
    }
}