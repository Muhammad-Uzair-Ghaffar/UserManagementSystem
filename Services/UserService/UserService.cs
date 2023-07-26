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
using UserManagementSystem.Context;
using UserManagementSystem.GenericRepository;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection;

namespace UserManagementSystem.Services.UserService
{

    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly IRepository<IdentityUser> _userRepository;
        private readonly AppDBContext _context;
        public UserService(AppDBContext context, IConfiguration configuration, IRepository<IdentityUser> userRepository)
        {
            _context = context;
            _configuration = configuration;
            _userRepository = userRepository;
        }
     

        public async Task<IdentityUser>  DeleteUser(string id)
        {
            IdentityUser user = await _userRepository.GetById(id);
            if (user == null)
            {
                throw new Exception("User does not exist");
            }
            _userRepository.Delete(user);
            await _userRepository.SaveChangesAsync();
            return user ;
        }

        public async Task<List<IdentityUser>>  GetAllUsers()
        {
            List<IdentityUser> dbusers= await _userRepository.GetAllAsync();
            return dbusers;
        }

        public async Task<IdentityUser> GetUserById(string id)
        {  
            IdentityUser dbuser = await _userRepository.GetById(id);
            return dbuser;
            
        }
        public async Task<List<IdentityUser>> GetUsersWithPagination(int page, int pageSize, string search, string sortBy, string sortOrder)
        {
            var query = _userRepository.GetQueryable();

            // Apply search filter
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(u => u.UserName.StartsWith(search));
            }

            // Apply sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                Type entityType = typeof(IdentityUser);

                // Check if the property exists in the entity type
                var propertyInfo = entityType.GetProperty(sortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo == null)
                {
                    throw new ArgumentException($"Sort by property '{sortBy}' is not a valid property for sorting");
                }
                switch (sortOrder.ToLower())
                {
                    case "asc":
                        query = query.OrderBy(u => EF.Property<object>(u, sortBy));
                        break;
                    case "desc":
                        query = query.OrderByDescending(u => EF.Property<object>(u, sortBy));
                        break;
                }
            }

            // Apply pagination
            var users = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return users;
        }

    }
}