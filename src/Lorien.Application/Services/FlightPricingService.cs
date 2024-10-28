using Lorien.Application.Exceptions;
using Lorien.Application.Mappers;
using Lorien.Application.Models.Request;
using Lorien.Application.Models.Response.Flights;
using Lorien.Domain.Interfaces.Repositories;
using Lorien.Domain.Interfaces.Services.Flights;
using Microsoft.Extensions.Logging;

namespace Lorien.Application.Services
{
    public class FlightPricingService(
        IAmadeusCRSHttpService _amadeusCRSHttpService,
        ICurrencyRepository _currencyRepository,
        IIATACodeRepository _iataCodeRepository,
        ILogger<FlightPricingService> _logger) : IFlightPricingService
    {
        public async Task<GetFlightPricingResponse> GetFlightPricingAsync(GetFlightPricingRequest request)
        {
            try
            {
                var mapped = request.MapToRequest();

                var flightOffers = await _amadeusCRSHttpService.GetFlightOffers(mapped);

                return flightOffers.MapToResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new CustomHttpException("An unexpected error has occurred: " + ex.Message);
            }
        }

        public Task<GetCurrenciesResponse> GetCurrencies()
        {
            return Task.FromResult(new GetCurrenciesResponse() { Currencies = _currencyRepository.Get() });
        }

        public Task<GetIATACodesResponse> GetIATACodes()
        {
            return Task.FromResult(new GetIATACodesResponse() { IATACodes = _iataCodeRepository.Get() });
        }
    }
}
