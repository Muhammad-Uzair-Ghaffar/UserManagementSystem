using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol;
using UserManagementSystem.Dtos.User;
using UserManagementSystem.Services.AccountService;
using UserManagementSystem.Services.EmailService;

namespace UserManagementSystem.Controllers
{

    [Route("api/v1/[controller]")]
    public class AccountController : BaseController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager; 
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailSender;
        private readonly IAccountService _accountService;

       

        public AccountController(UserManager<IdentityUser> userManager,
                              SignInManager<IdentityUser> signInManager, IConfiguration configuration, IMapper mapper, IEmailService emailSender, IAccountService accountService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _mapper = mapper;
            _accountService = accountService;
            _emailSender = emailSender;
        }
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
            {
                List<string> errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }
            try
            {
                IdentityUser user = _mapper.Map<IdentityUser>(model);

                var result = await _accountService.Register(user, model.Password);

                UserDto userDto = _mapper.Map<UserDto>(user);
                return Ok(userDto);
            }
            catch(Exception ex)
            {
                
                return BadRequest(ex.Message);
            }
            
        }
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto authDto)
        {
            if (!ModelState.IsValid)
            {
                List<string> errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }

            try
            {
                var token = await _accountService.Login(authDto.Email, authDto.Password);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ConfirmEmail/{token}")]
        public async Task<IActionResult> ConfirmEmail(string token)
        {
            try
            {
                var isConfirmed = await _accountService.ConfirmEmailAsync(token);
                if (isConfirmed)
                {
                    return Ok(null,"Email confirmed successfully.");
                }
                return BadRequest("Invalid token or user not found.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        private string CreateJwtToken(IdentityUser user)//, IList<string> roles
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Jwt:Secret").Value); 

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
    }
}