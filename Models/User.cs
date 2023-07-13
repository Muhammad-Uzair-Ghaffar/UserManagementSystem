using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagementSystem.Models
{
    public enum Role
    {
        admin = 1,
        customer = 2
    }
    public class User
    {

        public string Id { get; set; }
       
        public string Name { get; set; } 
        public int Age { get; set; }
        public string Email { get; set; }
       
        public Role Class { get; set; } 

        
    }
}