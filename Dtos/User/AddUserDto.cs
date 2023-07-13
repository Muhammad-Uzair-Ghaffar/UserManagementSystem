using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UserManagementSystem.Models;

namespace UserManagementSystem.Dtos.User
{
    public class AddUserDto
    {
        
        public string Name { get; set; }

        public int Age { get; set; }

        public string Email { get; set; }
        [EnumDataType(typeof(Role))]
        public Role Class { get; set; }
    }
}