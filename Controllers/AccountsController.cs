using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Nathan.Models;
using Nathan.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Nathan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {

        private readonly UserManager<UserModel> _userManager;
        //private readonly IRepository _repository;
        private readonly IUserClaimsPrincipalFactory<UserModel> _claimsPrincipalFactory;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AccountsController> _logger;

        public AccountsController(UserManager<UserModel> userManager, ILogger<AccountsController> logger, IUserClaimsPrincipalFactory<UserModel> claimsPrincipalFactory, RoleManager<IdentityRole> roleManager, IConfiguration configuration /*IRepository repository*/)
        {
            _userManager = userManager;
            _claimsPrincipalFactory = claimsPrincipalFactory;
            _configuration = configuration;
            //_repository = repository;
            _roleManager = roleManager;
            _logger = logger;
        }

        [HttpPost("register")]
            public async Task<IActionResult> RegisterUser([FromBody] RegisterViewModel model)
            {
                try {
                
                    if (model == null || string.IsNullOrEmpty(model.userName) || string.IsNullOrEmpty(model.password))
                    {
                        return BadRequest("Invalid request data.");
                    }


                    var user = await _userManager.FindByNameAsync(model.userName);

                    if (user == null)
                    {
                        var userId = Guid.NewGuid();

                        var newUser = new UserModel
                        {
                            Id = userId.ToString(),
                            UserName = model.userName
                        };

                        var result = await _userManager.CreateAsync(newUser, model.password);

                        if (result.Succeeded)
                        {
                            return NoContent();
                        }
                        else 
                        {
                        return BadRequest("User could not be added");
                        }
                    }
                    else
                    {
                        return BadRequest("Account already exists.");
                    }
                }

                catch (Exception ex)
                {
                    // Log the exception
                    _logger.LogError(ex, "An error occurred during user registration.");

                    return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error. Please contact support.");
                }



        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] RegisterViewModel request)
        {

            var user = await _userManager.FindByNameAsync(request.userName);


            if (user != null && await _userManager.CheckPasswordAsync(user, request.password))
            {
                try
                {
                    var token = GenerateJWTToken(request.userName);

                    // Return the JWT token as a response
                    return Ok(new { token });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred during user login.");

                    return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error. Please contact support.");
                }
            }


            // If authentication fails, return an error response
            return Unauthorized("Invalid username or password");
        }



        private string GenerateJWTToken(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, userName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Tokens:Issuer"],
                _configuration["Tokens:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }


    }

    
