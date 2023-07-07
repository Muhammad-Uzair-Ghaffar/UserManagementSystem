using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagementSystem.Models;

namespace UserManagementSystem.Dtos.User
{
    public class AddUserDto
    {
        public string Name { get; set; } = "Default username";
        public int age { get; set; }
        public string email { get; set; }="nomail";
        public DefaultUser Class { get; set; } = DefaultUser.customer;
    }
}