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
            new User(),
            new User {Name = "saim"}
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

        public async Task<List<User>>  DeleteUser(Guid id)
        {
           // ServiceResponse<List<GetUserDto>> serviceResponse = new ServiceResponse<List<GetUserDto>>();
            User user = users.First(c=> c.Id == id);
            users.Remove(user);
            return users ;
        }

        public async Task<List<User>>  GetAllUsers()
        {
            //ServiceResponse<List<GetUserDto>> serviceResponse = new ServiceResponse<List<GetUserDto>>();
            return users;
        }

        public async Task<User> GetUserById(Guid id)
        {
           // ServiceResponse<GetUserDto> serviceResponse = new ServiceResponse<GetUserDto>();
           
            return users.FirstOrDefault(c=> c.Id == id);
            
        }

        public async Task<List<User>> UpdateUser(Guid id, User newUser)
        {
          //  ServiceResponse<List<GetUserDto>> serviceResponse = new ServiceResponse<List<GetUserDto>>();
            User user = users.FirstOrDefault(c=> c.Id == id);
            user.Name=newUser.Name;
            user.Email=newUser.Email;
            user.Age=newUser.Age;
            user.Class=newUser.Class;
            Console.WriteLine(newUser.Name);
            return users; 
        }

       
    }
}