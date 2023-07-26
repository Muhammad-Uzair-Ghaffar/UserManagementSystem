using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace UserManagementSystem.Services.AccountService
{
    public class AccountService : IAccountService
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;

        public AccountService(UserManager<IdentityUser> userManager,SignInManager<IdentityUser> signInManager, IEmailSender emailSender, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _emailSender = emailSender;
        }

        public async Task<bool> ConfirmEmailAsync(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Secret").Value);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero // No tolerance for token expiration
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = jwtToken.Claims.FirstOrDefault(x => x.Type == "nameid")?.Value;


            if (userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    if (user.EmailConfirmed == true)
                    {
                        throw new Exception("Email Already confirmed ");
                    }
                    user.EmailConfirmed = true;
                    await _userManager.UpdateAsync(user);

                    return true; // Email confirmed successfully.
                }
            }

            return false; // Invalid token or user not found.
        }
    

        private string CreateJwtToken(IdentityUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Secret").Value);

            var tokenDescriptor = new SecurityTokenDescriptor
            {

                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                    //new Claim(ClaimTypes.Role, string.Join(",", roles)),
                }),
                Expires = DateTime.UtcNow.AddDays(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key)
, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    

        public async Task<string> Login(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, password, false, false);

            if (result.Succeeded)
            {
                var token = CreateJwtToken(user);
                return token.ToString();
            }

            throw new Exception("Login failed.");
        }

        public async Task<IdentityUser> Register(IdentityUser user,string  password)
        {
            string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
            if (!Regex.IsMatch(password, passwordPattern))
            {
                throw new Exception("Invalid password pattern. Password must be at least 8 characters long and contain special characters.");
            }
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var jwtToken = CreateJwtToken(user);
                var baseUrl = _configuration.GetSection("AppSettings:BaseUrl").Value;

                var confirmationLink = baseUrl + "/" + "Account" + "/" + "ConfirmEmail/" + jwtToken;
                await _emailSender.SendEmailAsync(user.Email, "Confirm your email", $"Please confirm your account by clicking this link: {confirmationLink}");
                return user;
            }
            var errorDescriptions = string.Join(Environment.NewLine, result.Errors.Select(error => error.Description));


            throw new Exception("Email or username Duplicate");//can use a custom exception that will send list or this error descriptions which is a list of errors combined together 
           
        }
    }
}
