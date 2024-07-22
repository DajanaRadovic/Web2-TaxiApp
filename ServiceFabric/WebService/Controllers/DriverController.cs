using Common.DTOs;
using Common.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebService.Helpers;
using WebService.InterfaceWebService;

namespace WebService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
       
        private readonly IDriverService _driverService;
        private  IConfiguration _config;
        private readonly IEmail email;

        public DriverController(IDriverService driverService, IConfiguration configuration, IEmail email) {
            _driverService = driverService;
            _config = configuration;
            this.email = email;
        }

        [Authorize(Policy = "Driver")]
        [HttpGet]
        public async Task<IActionResult> GetNoFinishedRides() {
            try
            {
                var res = await _driverService.GetAllNotFinishedRides();

                if (res != null && res.Count > 0)
                {
                    var response = new
                    {
                        rides = res,
                        message = "Successfully retrieved list of uncompleted rides"
                    };
                    return Ok(response);
                }
                else
                {
                    return BadRequest("No uncompleted rides found");
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving rides");
            }
        }

        [Authorize(Policy = "Driver")]
        [HttpGet]
        public async Task<IActionResult> GetFinishedRidesDriver([FromQuery] Guid id) {
            try
            {
                var res = await _driverService.GetFinishedRidesDriver(id);

                if (res != null && res.Count > 0)
                {
                    var resp = new
                    {
                        rides = res,
                        message = "Successfully"
                    };
                    return Ok(resp);
                }
                else
                {
                    return BadRequest("No completed rides found for this driver");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving completed rides: {ex.Message}");
            }
        }

        [Authorize(Policy = "Driver")]
        [HttpGet]
        public async Task<IActionResult> GetCurrentRideDriver(Guid id) {
            try
            {
                var res = await _driverService.GetCurrentRideDriver(id);

                if (res != null)
                {
                    var response = new
                    {
                        trip = res,
                        message = "Successfully"
                    };
                    return Ok(response);
                }
                else
                {
                    return BadRequest("No current ride found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving current ride: {ex.Message}");
            }
        }


        [Authorize(Policy = "Driver")]
        [HttpPut]
        public async Task<IActionResult> AcceptNewDrive([FromBody] AcceptRideDTO ride) {
            try
            {
                var res = await _driverService.AcceptNewDrive(ride);

                if (res != null)
                {
                    var response = new
                    {
                        ride = res,
                        message = "Successfully"
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
    }
}
