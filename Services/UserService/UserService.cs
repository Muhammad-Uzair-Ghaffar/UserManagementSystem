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

        public async Task<ServiceResponse<List<User>>> AddUser(User newUser)
        {
            ServiceResponse<List<User>> serviceResponse = new ServiceResponse<List<User>>();
            users.Add(newUser);
            serviceResponse.Data = users;
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<User>>>  DeleteUser(int id)
        {
            ServiceResponse<List<User>> serviceResponse = new ServiceResponse<List<User>>();
            User user = users.First(c=> c.Id == id);
            users.Remove(user);
            serviceResponse.Data = users;
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<User>>>  GetAllUsers()
        {
            ServiceResponse<List<User>> serviceResponse = new ServiceResponse<List<User>>();
            serviceResponse.Data = users;
            return serviceResponse;
        }

        public async Task<ServiceResponse<User>> GetUserById(int id)
        {
            ServiceResponse<User> serviceResponse = new ServiceResponse<User>();
            serviceResponse.Data = users.FirstOrDefault(c=> c.Id == id);
            return serviceResponse;
            
        }

        public async Task<ServiceResponse<List<User>>>  UpdateUser(int id, User newUser)
        {
            ServiceResponse<List<User>> serviceResponse = new ServiceResponse<List<User>>();
            User user = users.FirstOrDefault(c=> c.Id == id);
            user.Name=newUser.Name;
            user.Email=newUser.Email;
            user.Age=newUser.Age;
            user.Class=newUser.Class;
            Console.WriteLine(newUser.Name);
            serviceResponse.Data=users;
            return serviceResponse;
        }

       
    }
}