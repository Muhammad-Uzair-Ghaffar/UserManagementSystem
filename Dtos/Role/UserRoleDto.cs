using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagementSystem.Models;

namespace UserManagementSystem.Dtos.Role
{
    public class UserRoleDto
    {
        public string? Id { get; set; }
        public string UserId { get; set; }
        public string RoleId { get; set; }
    }
}