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
    public class ServiceResponse<T>
    {
        public T Data { get; set; }

        public Status status  { get; set; } = Status.Success;

        public string Message { get; set; } = null;
    }
}