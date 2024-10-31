using Lorien.Application.Exceptions;
using Lorien.Application.Mappers;
using Lorien.Application.Models.Request;
using Lorien.Application.Models.Response.Flights;
using Lorien.Configuration;
using Lorien.Domain.Interfaces.Services.Caching;
using Lorien.Domain.Interfaces.Services.Flights;
using Lorien.Domain.Models.Caching;
using Lorien.Domain.Models.Data;
using Lorien.Domain.Models.Response.Flights;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Lorien.Application.Services.Flights
{
    public class FlightPricingService(
        IAmadeusCRSHttpService amadeusCRSHttpService,
        ICachingService cachingService,
        ILogger<FlightPricingService> logger,
        IOptions<LorienSettings> options) : IFlightPricingService
    {
        private readonly LorienSettings settings = options.Value;

        public async Task<GetFlightPricingResponse> GetFlightPricingAsync(GetFlightPricingRequest request)
        {
            try
            {
                string cacheKey = request.ToString();

                var mappedRequest = request.MapToRequest();

                var cachedOffers = cachingService.Get<IEnumerable<FlightOfferData>>(cacheKey);

                if (cachedOffers == null || !cachedOffers.Any())
                {
                    GetFlightOfferResponse flightOffersResponse = await amadeusCRSHttpService.GetFlightOffers(mappedRequest);

                    if (flightOffersResponse.Data.Any())
                    {
                        cachingService.Add(cacheKey, flightOffersResponse.Data);
                    }

                    return flightOffersResponse.MapToResponse();
                }

                return new GetFlightOfferResponse() { Data = cachedOffers }.MapToResponse();
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving flight prices. {ex.Message}");
                throw new CustomHttpException("An unexpected error has occurred: " + ex.Message);
            }
        }

        public Task<GetCurrenciesResponse> GetCurrencies()
        {
            try
            {
                var currencies = cachingService
                    .Get<CacheObject<Currency>>(nameof(CacheKeys.Currencies))!
                    .Items;

                return Task.FromResult(currencies!.MapToResponse());
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw new CustomHttpException("An unexpected error has occurred: " + ex.Message);
            }
        }

        public Task<GetIATACodesResponse> GetIATACodes(GetIATACodesRequest request)
        {
            try
            {
                var codes = cachingService
                    .Get<CacheObject<IATACode>>(nameof(CacheKeys.IATACodes))!
                    .Items
                    .Where(x => x.IATA.StartsWith(request.Input) || x.Name.StartsWith(request.Input))
                    .Take(settings.MaxIATACodeResponseSize);

                return Task.FromResult(codes!.MapToResponse());
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw new CustomHttpException("An unexpected error has occurred: " + ex.Message);
            }
        }
    }
}
