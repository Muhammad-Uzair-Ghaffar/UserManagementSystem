using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagementSystem.Models
{   
    public enum Status
    {
        Success= 1,
        Failed = 0,
    }
    public class ServiceResponse
    {
        public object Data { get; set; }

        public Status Status  { get; set; } 

        public string Message { get; set; } = null;
    }
}