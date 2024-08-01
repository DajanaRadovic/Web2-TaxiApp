using Common.DTOs;
using Common.Interface;
using Common.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using System.Security.Claims;
using System.Fabric;
using System.IdentityModel.Tokens.Jwt;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using WebService.Helpers;
using WebService.InterfaceWebService;

namespace WebService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ValidationServices _userValidationService;
        private readonly IUserService _userService;
        private IConfiguration _configuration;
        private readonly Common.Interface.IEmail email;

        public UserController(ValidationServices validation,IUserService userService,IConfiguration configuration, IEmail email)
        {
            _userValidationService = validation;
            _userService = userService;
            _configuration = configuration;
            this.email = email;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] Signup user) {

            var validationResult = _userValidationService.ValidateUserRegistration(user);
            if (validationResult != null)
            {
                return validationResult;
            }

            try
            {
                var result = await _userService.RegisterUser(user);
                if (result)
                {
                    return Ok($"Successfully registered: {user.Username}");
                }
                else
                {
                    return Conflict("User already exists in database!");
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while registering new User");
            }

        }

        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers() {
            try
            {
                var users = await _userService.GetUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching users: {ex.Message}");
            }
        }

        //OVDE IDE LOGIN
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDTO user)
        {
            if (string.IsNullOrEmpty(user.Email) || !IsValidEmail(user.Email)) return BadRequest("Invalid email format");
            if (string.IsNullOrEmpty(user.Password)) return BadRequest("Password cannot be null or empty");

            try
            {
                var fabricClient = new FabricClient();
                AuthenticatedUserDTO result = null; // Initialize result to null

                var list = await fabricClient.QueryManager.GetPartitionListAsync(new Uri("fabric:/ServiceFabric/UserService"));
                foreach (var partition in list)
                {
                    var partitionKey = new ServicePartitionKey(((Int64RangePartitionInformation)partition.PartitionInformation).LowKey);
                    var proxy = ServiceProxy.Create<IUser>(new Uri("fabric:/ServiceFabric/UserService"), partitionKey);
                    var res = await proxy.LoginUser(user);

                    if (res != null)
                    {
                        result = res;
                        break;
                    }
                }

                if (result != null)
                {
                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim("MyCustomClaim", result.Roles.ToString()));

                    var Sectoken = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                        _configuration["Jwt:Issuer"],
                        claims,
                        expires: DateTime.Now.AddMinutes(360),
                        signingCredentials: credentials);

                    var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

                    var response = new
                    {
                        token = token,
                        user = result,
                        message = "Login successful"
                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest("Incorrect email or password");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while login User");
            }
        }

        private bool IsValidEmail(string email)
        {
            const string pattern = @"^[^\s@]+@[^\s@]+\.[^\s@]+$";
            return Regex.IsMatch(email, pattern);
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetUserDetails([FromQuery] Guid id) {
            try
            {
                var userDetails = await _userService.GetUserDetails(id);

                if (userDetails != null)
                {
                    var response = new
                    {
                        user = userDetails,
                        message = "Successfully"
                    };
                    return Ok(response);
                }
                else
                {
                    return BadRequest("This id does not exist");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving user details: {ex.Message}");
            }
        }

        [AllowAnonymous]
        [HttpPut]
        public async Task<IActionResult> ChangeUser([FromForm] UpdateUser user) {
          
            try
            {
                Console.WriteLine($"Received Id: {user.Id}");
                Console.WriteLine($"Received FirstName: {user.FirstName}");
                Console.WriteLine($"Received LastName: {user.LastName}");
                Console.WriteLine($"Received Birthday: {user.Birthday}");
                Console.WriteLine($"Received Email: {user.Email}");
                Console.WriteLine($"Received Username: {user.Username}");
                Console.WriteLine($"Received Address: {user.Address}");
                Console.WriteLine($"Received Image: {user.Image?.FileName}");

                var res = await _userService.UpdateUser(user);

                if (res != null)
                {
                    var resp = new
                    {
                        user = res,
                        message = "Successfully"
                    };
                    return Ok(resp);
                }
                else
                {
                    return BadRequest("This id does not exist");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving user details: {ex.Message}");
            }
        }


    }

       
}
