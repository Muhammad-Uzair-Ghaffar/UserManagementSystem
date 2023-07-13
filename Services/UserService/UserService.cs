using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using UserManagementSystem.Dtos.User;
using UserManagementSystem.Models;

namespace UserManagementSystem.Services.UserService
{
    public class UserService : IUserService
    {
        private static List<User> users = new List<User>
        {
            new User{Id= Guid.NewGuid().ToString()},
            new User {Id= Guid.NewGuid().ToString(),Name = "saim"}
        }; 
        private readonly IMapper _mapper;

        public UserService(IMapper mapper)
        {
            _mapper = mapper;
        }
        public async Task<List<User>> AddUser(User newUser)
        {
            users.Add(newUser);
            return users;
        }

        public async Task<List<User>>  DeleteUser(string id)
        {
            User user = users.First(c=> c.Id == id);
            users.Remove(user);
            return users ;
        }

        public async Task<List<User>>  GetAllUsers()
        {
            return users;
        }

        public async Task<User> GetUserById(string id)
        {  
            return users.First(c=> c.Id == id);
            
        }

        public async Task<List<User>> UpdateUser(string id, User newUser)
        {
            User user = users.First(c=> c.Id == id);
            user.Name=newUser.Name;
            user.Email=newUser.Email;
            user.Age=newUser.Age;
            user.Class=newUser.Class;
            return users; 
        }

       
    }
}