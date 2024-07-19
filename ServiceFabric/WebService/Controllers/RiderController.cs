using Common.Interface;
using Common.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using System.Diagnostics.Eventing.Reader;

namespace WebService.Controllers
{
   
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RiderController : ControllerBase
    {

        [Authorize(Policy = "Rider")]
        [HttpGet]
        public async Task<IActionResult> GetPrice([FromQuery] Location location) {

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
    
    }
}
