using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagementSystem.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Default username";
        public int age { get; set; }
        public string email { get; set; }="nomail";
        public DefaultUser Class { get; set; } = DefaultUser.customer;
    }
}