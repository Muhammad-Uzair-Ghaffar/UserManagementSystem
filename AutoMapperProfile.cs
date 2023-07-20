using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using UserManagementSystem.Dtos.Role;
using UserManagementSystem.Dtos.User;
using UserManagementSystem.Models;

namespace UserManagementSystem
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<IdentityUser,UserDto>();
            CreateMap<UserDto, IdentityUser>();
            CreateMap<IdentityUser, LoginDto>();
            CreateMap<LoginDto, IdentityUser>();
        }
    }
}