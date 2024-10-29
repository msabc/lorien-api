using Lorien.Api.Controllers.Base;
using Lorien.Application.Models.Request;
using Lorien.Application.Services.Flights;
using Microsoft.AspNetCore.Mvc;

namespace Lorien.Api.Controllers
{
    [Route("flight-pricing")]
    public class FlightPricingController(IFlightPricingService _flightService) : LorienBaseController
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
