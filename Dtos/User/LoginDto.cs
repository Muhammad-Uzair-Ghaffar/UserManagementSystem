using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UserManagementSystem.Models;

namespace UserManagementSystem.Dtos.User
{
    public class LoginDto 
    {
        [EmailAddress]
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}