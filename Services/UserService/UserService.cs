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
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UserManagementSystem.Data;
using UserManagementSystem.Dtos.User;
using UserManagementSystem.Models;

namespace UserManagementSystem.Services.UserService
{
    
    public class UserService : IUserService
    {
        private static List<User> users = new List<User>();
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;
        public UserService(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<List<User>> AddUser(User newUser)
        {
            bool emailExists = await _context.Users.AnyAsync(c => c.Email == newUser.Email);
            if (emailExists)
            {
                throw new Exception("Email already exists");
            }
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            users.Add(newUser);
            return await _context.Users.ToListAsync();
        }

        public async Task<User>  DeleteUser(string id)
        {
            User user = await _context.Users.FirstAsync(c=> c.Id == id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return user ;
        }

        public async Task<List<User>>  GetAllUsers()
        {
            List<User> dbusers= await _context.Users.ToListAsync();
            return dbusers;
        }

        public async Task<User> GetUserById(string id)
        {  
            User dbuser = await _context.Users.FirstAsync(c => c.Id == id);
            return dbuser;
            
        }
        
        public async Task<string> Login(string useremail, string password)
        {
            User user = await _context.Users.FirstOrDefaultAsync(c =>c.Email.Equals(useremail));
            if (user == null)
            {
                throw new Exception("Wrong credentials");
            }
            else if(!VerifyPassword(password,user.PasswordHash, user.PasswordSalt))
            {
                throw new Exception("Wrong Password");
            }
            else
            {
                return CreateJwtToken(user);
            }

        }

        public async Task<User> Register(User newUser, string password)
        {
            if (newUser.Name=="")
            {
                throw new Exception("Name cannot be empty");
            }
            string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
            if (!Regex.IsMatch(password, passwordPattern))
            {
                throw new Exception("Invalid password pattern. Password must be at least 8 characters long and contain special characters.");
            }
            if (await UserExists(newUser.Email))
            {
                throw new Exception("A user with the same email already exists.");
            }
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            newUser.PasswordHash= passwordHash;
            newUser.PasswordSalt= passwordSalt;
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
            return newUser;
        }

        public async Task<User> UpdateUser(string id, User newUser)
        {
            User user = users.First(c=> c.Id == id);
            user.Name=newUser.Name;
            user.Email=newUser.Email;
            user.Age=newUser.Age;
            return user; 
        }

        public async Task<bool> UserExists(string useremail)
        {
            if(await _context.Users.AnyAsync(c=> c.Email== useremail))
            {
                return true;
            }
            return false;
        }

        private void CreatePasswordHash(string passwod, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA256())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(passwod));
            }
        }

        private bool VerifyPassword(string passwod, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA256(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(passwod));
                for(int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        private string CreateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name)
            };
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:Secret").Value)
            );
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
           
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

    }
}