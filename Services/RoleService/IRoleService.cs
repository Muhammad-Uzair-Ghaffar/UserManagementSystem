//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using UserManagementSystem.Models;
//using UserManagementSystem.Dtos.User;

//namespace UserManagementSystem.Services.RoleService
//{
//    public interface IRoleService
//    {
//        Task<List<Role>> GetAllRoles();
//        Task<List<UserRole>> GetAllUserRoles();
//        Task<List<Role>> AddRole(Role newRole,string userId);
//        Task<UserRole> AddUserRole(UserRole newUserrole);
//        Task DeleteRole(string id);
//        Task DeleteUserRole(string userId, string roleId);
//        Task IsAdmin(string userId);

//    }
//}