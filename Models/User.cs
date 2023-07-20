using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagementSystem.Models
{
   
    public class User
    {

        public string Id { get; set; }
       
        public string Name { get; set; } 
        public string FatherName { get; set; } 
        public int Age { get; set; }
        public string Email { get; set; }

        public byte[] PasswordHash  { get; set; }
        public byte[] PasswordSalt  { get; set; }

      
    }
}