using Microsoft.AspNetCore.Identity;
using UserManagementSystem.Dtos.User;

namespace UserManagementSystem.Services.AccountService
{
    public interface IAccountService
    {

        Task<IdentityUser> RegisterUser(LoginDto model);
        Task<bool> ConfirmEmailAsync(string token);//will take user id from http context accessor here 
       
        Task<string> Login(string email, string password);
        Task<string> CreateJwtToken(IdentityUser user);


    }
}
