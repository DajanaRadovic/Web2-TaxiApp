using Common.Interface;
using Common.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using System.Diagnostics.Eventing.Reader;
using WebService.Helpers;
using WebService.InterfaceWebService;

namespace WebService.Controllers
{

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RiderController : ControllerBase
    {
        private readonly IRiderService _riderService;
        private IConfiguration _configuration;
        private readonly IEmail email;

        public RiderController(IRiderService riderService, IConfiguration configuration, IEmail email) {
            _riderService = riderService;
            _configuration = configuration;
            this.email = email;
        }

        [Authorize(Policy = "Rider")]
        [HttpGet]
        public async Task<IActionResult> GetPrice([FromQuery] Location location)
        {

            Calculation calculation = await ServiceProxy.Create<ICalculation>(new Uri("fabric:/ServiceFabric/PriceCalculationService")).GetPrice(location.ToLocation, location.FromLocation);
            if (calculation != null)
            {

                var response = new
                {
                    price = calculation,
                    message = "Succesfuly"
                };
                return Ok(response);
            }
            else
            {
                return StatusCode(500, "An error occurred while estimating price");
            }
        }

        [Authorize(Policy = "Rider")]
        [HttpGet]
        public async Task<IActionResult> GetFinishedRidesRider([FromQuery] Guid id)
        {
            try
            {
                var res = await _riderService.GetFinishedRidesRider(id);

                if (res != null && res.Count > 0)
                {
                    var response = new
                    {
                        rides = res,
                        message = "Successfully"
                    };
                    return Ok(response);
                }
                else
                {
                    return BadRequest("No completed rides found for this rider");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving completed rides: {ex.Message}");
            }
        }

        [Authorize(Policy = "Rider")]
        [HttpPut]
        public async Task<IActionResult> SubmitRating([FromBody] Rate rate)
        {
            try
            {
                bool result = await _riderService.Rating(rate.IdDrive, rate.Rating);

                if (result)
                {
                    return Ok("Successfully");
                }
                else
                {
                    return BadRequest("Rating is not submitted");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while submitting rating: {ex.Message}");
            }

        }

        [Authorize(Policy = "Rider")]
        [HttpGet]
        public async Task<IActionResult> GetNotRated()
        {
            try
            {
                var res = await _riderService.GetNotRated();

                if (res != null)
                {
                    var response = new
                    {
                        rides = res,
                        message = "Successfully"
                    };
                    return Ok(response);
                }
                else
                {
                    return BadRequest("No unrated rides found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving unrated rides: {ex.Message}");
            }
        }

        [Authorize(Policy = "Rider")]
        [HttpGet]
        public async Task<IActionResult> GetCurrentDrive(Guid id) {
            try
            {
                var res = await _riderService.GetCurrentDrive(id);

                if (res != null)
                {
                    var response = new
                    {
                        drive = res,
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


        [Authorize(Policy = "Rider")]
        [HttpPut]
        public async Task<IActionResult> AcceptGivenDrive([FromBody] AcceptRides acceptedRides) {
            try
            {
                var result = await _riderService.AcceptGivenDrive(acceptedRides);

                if (result != null)
                {
                    var res = new
                    {
                        Drive = result,
                        message = "Successfully"
                    };
                    return Ok(res);
                }
                else
                {
                    return BadRequest("You already submitted ticket!");
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while accepting new drive!");
            }
        }
    }

}
