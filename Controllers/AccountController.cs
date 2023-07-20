using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using UserManagementSystem.Dtos.User;

namespace UserManagementSystem.Controllers
{
    
    [Route("[controller]")]
    public class AccountController : BaseController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager; 
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        
        public AccountController(UserManager<IdentityUser> userManager,
                              SignInManager<IdentityUser> signInManager, IConfiguration configuration, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _mapper = mapper;
        }
       
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] LoginDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = new IdentityUser
                    {
                        UserName = model.UserName,
                        Email = model.Email,
                    };
                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        UserDto userDto = _mapper.Map<UserDto>(user);
                        return Ok(userDto);

                    }
                    var errorDescriptions = result.Errors.Select(error => error.Description).ToList();
                    return BadRequest(errorDescriptions); 

                }
                else
                {
                    List<string> errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return BadRequest(errors);
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    return BadRequest(ex.Message); 
                }
                else
                {
                    return BadRequest(ex.InnerException.Message);
                }

            }
        }
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto authDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByEmailAsync(authDto.Email); 
                    if (user == null)
                    {
                        return BadRequest("User Not Found"); 
                    }
                    var result = await _signInManager.PasswordSignInAsync(user.UserName, authDto.Password, false, false); //

                    if (result.Succeeded)
                    {
                        var token = CreateJwtToken(user); 
                        return Ok(new { Token = token });
                    }
                    return BadRequest(null,"Login failed");

                }
                else
                {
                    List<string> errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return BadRequest(errors);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        private string CreateJwtToken(IdentityUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("JWT:Secret").Value); 

            var tokenDescriptor = new SecurityTokenDescriptor
            {
            
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
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