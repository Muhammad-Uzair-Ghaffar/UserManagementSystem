using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagementSystem.Models
{   
    public enum Status
    {
        Success= 200,
        BadRequest = 400,
        IncorrectUrl= 404
    }
    public class ServiceResponse
    {
        public object Data { get; set; }

        public Status Status  { get; set; } 

        public string Message { get; set; } = null;
    }
}