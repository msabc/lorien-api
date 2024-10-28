using Lorien.Application.Models.Request;
using Lorien.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lorien.Api.Controllers
{
    [ApiController]
    [Route("flight-pricing")]
    public class FlightPricingController(IFlightPricingService _flightService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetFlightPrices([FromQuery] GetFlightPricingRequest request)
        {
            return Ok(await _flightService.GetFlightPricingAsync(request));
        }

        [HttpGet("currency-codes")]
        public async Task<IActionResult> GetCurrencyCodes()
        {
            return Ok(await _flightService.GetCurrencies());
        }

        [HttpGet("iata-codes")]
        public async Task<IActionResult> GetIATACodes()
        {
            return Ok(await _flightService.GetIATACodes());
        }
    }
}
