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
using UserManagementSystem.Services;
using UserManagementSystem.Services.AccountService;

namespace UserManagementSystem.Controllers
{

    [Route("api/v1/[controller]")]
    public class AccountController : BaseController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager; 
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly IAccountService _accountService;

       

        public AccountController(UserManager<IdentityUser> userManager,
                              SignInManager<IdentityUser> signInManager, IConfiguration configuration, IMapper mapper, IEmailSender emailSender, IAccountService accountService)
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
            //        if (ModelState.IsValid)
            //    {

            //        var user = new IdentityUser
            //        {
            //            UserName = model.UserName,
            //            Email = model.Email,
            //        };
            //        var result = await _userManager.CreateAsync(user, model.Password);

            //        if (result.Succeeded)
            //        {
            //            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //            var jwtToken = CreateJwtToken(user);
            //            var baseUrl = _configuration.GetSection("AppSettings:BaseUrl").Value;

            //            var confirmationLink = baseUrl + "/" + "Account" + "/" + "ConfirmEmail/" + jwtToken;
            //            await _emailSender.SendEmailAsync(user.Email, "Confirm your email", $"Please confirm your account by clicking this link: {confirmationLink}");
            //            UserDto userDto = _mapper.Map<UserDto>(user);
            //            return Ok(userDto);

            //        }
            //        var errorDescriptions = result.Errors.Select(error => error.Description).ToList();
            //        return BadRequest(errorDescriptions); 

            //    }
            //    else
            //    {
            //        List<string> errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            //        return BadRequest(errors);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    return BadRequest(ex.Message); 

            //}
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
                    return Ok("Email confirmed successfully.");
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
    }
}