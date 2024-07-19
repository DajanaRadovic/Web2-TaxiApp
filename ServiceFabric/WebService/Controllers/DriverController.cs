using Common.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebService.InterfaceWebService;

namespace WebService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DriverController : ControllerBase

    {
        private readonly IDriverService _driverService;

        [Authorize(Policy = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetDrivers() {
            try
            {
                var drivers = await _driverService.AllDrivers();

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

        public async Task<IActionResult> ChangeStatusDriver([FromBody] DriverStatusModificationDTO driver) {
            try
            {
                var res = await _driverService.ChangeDriverStatus(driver.Id, driver.Status);

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
    }
}
