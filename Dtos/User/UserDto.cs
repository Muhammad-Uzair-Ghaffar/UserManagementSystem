using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UserManagementSystem.Models;

namespace UserManagementSystem.Dtos.User
{
    public class UserDto
    {

        public string? Id { get; set; }
        [Required(ErrorMessage = "Name is cannot be nullable.")]
        
        public string UserName { get; set; }

       
        [EmailAddress]
        public string Email { get; set; }
       
    }
}