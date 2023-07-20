using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UserManagementSystem.Dtos.User;
using UserManagementSystem.Models;
using UserManagementSystem.Models.AppDBContext;

namespace UserManagementSystem.Services.UserService
{
    
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly AppDBContext _context;
        public UserService(AppDBContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        //public async Task<List<IdentityUser>> AddUser(IdentityUser newUser)
        //{
        //    bool emailExists = await _context.Users.AnyAsync(c => c.Email == newUser.Email);
        //    if (emailExists)
        //    {
        //        throw new Exception("Email already exists");
        //    }
        //    await _context.Users.AddAsync(newUser);
        //    await _context.SaveChangesAsync();

        //    return await _context.Users.ToListAsync();
        //}

        public async Task<IdentityUser>  DeleteUser(string id)
        {
            IdentityUser user = await _context.Users.FirstAsync(c=> c.Id == id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return user ;
        }

        public async Task<List<IdentityUser>>  GetAllUsers()
        {
            List<IdentityUser> dbusers= await _context.Users.ToListAsync();
            return dbusers;
        }

        public async Task<IdentityUser> GetUserById(string id)
        {  
            IdentityUser dbuser = await _context.Users.FirstAsync(c => c.Id == id);
            return dbuser;
            
        }

      
        //public async Task<User> UpdateUser(string id, User newUser)
        //{
        //    User user = users.First(c=> c.Id == id);
        //    user.Name=newUser.Name;
        //    user.Email=newUser.Email;
        //    user.Age=newUser.Age;
        //    return user; 
        //}


    }
}