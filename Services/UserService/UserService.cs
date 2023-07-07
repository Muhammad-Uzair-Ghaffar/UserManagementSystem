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
            new User { Id = 1,Name = "saim"}
        }; 
        private readonly IMapper _mapper;

        public UserService(IMapper mapper)
        {
            _mapper = mapper;
        }
        public async Task<ServiceResponse<List<GetUserDto>>> AddUser(AddUserDto newUser)
        {
            ServiceResponse<List<GetUserDto>> serviceResponse = new ServiceResponse<List<GetUserDto>>();
            User user =_mapper.Map<User>(newUser);
            user.Id=users.Max(c => c.Id)+1;
            users.Add(user);
            serviceResponse.Data = (users.Select(c => _mapper.Map<GetUserDto>(c))).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetUserDto>>>  DeleteUser(int id)
        {
            ServiceResponse<List<GetUserDto>> serviceResponse = new ServiceResponse<List<GetUserDto>>();
            User user = users.First(c=> c.Id == id);
            users.Remove(user);
            serviceResponse.Data = (users.Select(c => _mapper.Map<GetUserDto>(c))).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetUserDto>>>  GetAllUsers()
        {
            ServiceResponse<List<GetUserDto>> serviceResponse = new ServiceResponse<List<GetUserDto>>();
            serviceResponse.Data = (users.Select(c => _mapper.Map<GetUserDto>(c))).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetUserDto>> GetUserById(int id)
        {
            ServiceResponse<GetUserDto> serviceResponse = new ServiceResponse<GetUserDto>();
            serviceResponse.Data = _mapper.Map<GetUserDto>(users.FirstOrDefault(c=> c.Id == id));
            return serviceResponse;
            
        }

        public async Task<ServiceResponse<List<GetUserDto>>>  UpdateUser(int id, AddUserDto newUser)
        {
            ServiceResponse<List<GetUserDto>> serviceResponse = new ServiceResponse<List<GetUserDto>>();
            User user = users.FirstOrDefault(c=> c.Id == id);
            user.Name=newUser.Name;
            user.Email=newUser.Email;
            user.Age=newUser.Age;
            user.Class=newUser.Class;
            Console.WriteLine(newUser.Name);
            serviceResponse.Data=(users.Select(c => _mapper.Map<GetUserDto>(c))).ToList();
            return serviceResponse;
        }

       
    }
}