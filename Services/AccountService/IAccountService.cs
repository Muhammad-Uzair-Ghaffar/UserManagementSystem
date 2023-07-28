using Microsoft.AspNetCore.Identity;
using UserManagementSystem.Dtos.User;

namespace UserManagementSystem.Services.AccountService
{
    public interface IAccountService
    {

        Task<IdentityUser> Register(IdentityUser user, string password);
        Task<bool> ConfirmEmailAsync(string token);
       
        Task<string> Login(string email, string password);


    }
}
