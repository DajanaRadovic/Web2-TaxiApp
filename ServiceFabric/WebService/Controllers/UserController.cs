using Common.DTOs;
using Common.Interface;
using Common.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using System.Fabric;
using System.Security.AccessControl;
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
                    return Ok($"Successfully registered new User: {user.Username}");
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
