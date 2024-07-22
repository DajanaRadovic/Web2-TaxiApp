using Common.DTOs;
using Common.Interface;
using Common.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebService.Helpers;
using WebService.InterfaceWebService;

namespace WebService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdminController : ControllerBase

    {
        private IConfiguration _configuration;
        private readonly IAdminService _adminService;
        private readonly IEmail email;

        public AdminController(IAdminService adminService, IConfiguration configuration, IEmail email) {
            _adminService = adminService;
            _configuration = configuration;
            this.email = email;
        }




        [Authorize(Policy = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetDrivers()
        {
            try
            {
                var drivers = await _adminService.AllDrivers();

                if (drivers != null)
                {
                    var response = new
                    {
                        user = drivers,
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

        [Authorize(Policy = "Admin")]
        [HttpPut]

        public async Task<IActionResult> ChangeStatusDriver([FromBody] DriverStatusModificationDTO driver)
        {
            try
            {
                var res = await _adminService.ChangeDriverStatus(driver.Id, driver.Status);

                if (res)
                {
                    return Ok("Driver status updated successfully.");
                }
                else
                {
                    return BadRequest("Failed to update driver status.");
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while updating driver status");
            }
        }



        [Authorize(Policy = "Admin")]
        [HttpPut]
        public async Task<IActionResult> VerifyDriver([FromBody] DriverVerificationStatusDTO driver)
        {
            try
            {
                var result = await _adminService.VerifyDriver(driver.Id, driver.Email, driver.Task);

                if (result)
                {
                    var response = new
                    {
                        Verified = result,
                        message = $"Driver with id:{driver.Id} , status:{driver.Task}"
                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest("This id does not exist");
                }
            }
            catch
            {
                return BadRequest("Something went wrong!");
            }
        }



        [Authorize(Policy = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetCompletedRidesForAdmin()
        {
            try
            {
                var res = await _adminService.DriverForVerification();

                if (res != null && res.Count > 0)
                {
                    var resp = new
                    {
                        rides = res,
                        message = "Successfully"
                    };
                    return Ok(res);
                }
                else
                {
                    return BadRequest("No completed rides found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving completed rides: {ex.Message}");
            }
        }

        [Authorize(Policy = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetDriversForVerification()
        {
            try
            {
                var result = await _adminService.GetFinishedRidesAdmin();

                if (result != null)
                {
                    var response = new
                    {
                        drivers = result,
                        message = "Successfully retrieved list of drivers"
                    };
                    return Ok(response);
                }
                else
                {
                    return BadRequest("No drivers found for verification");
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving drivers for verification");
            }
        }
    }
}
