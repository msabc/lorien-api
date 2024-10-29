using Lorien.Application.Exceptions;
using Lorien.Application.Mappers;
using Lorien.Application.Models.Request;
using Lorien.Application.Models.Response.Flights;
using Lorien.Domain.Interfaces.Repositories;
using Lorien.Domain.Interfaces.Services.Caching;
using Lorien.Domain.Interfaces.Services.Flights;
using Lorien.Domain.Models.Caching;
using Lorien.Domain.Models.Response.Flights;
using Microsoft.Extensions.Logging;

namespace Lorien.Application.Services.Flights
{
    public class FlightPricingService(
        IAmadeusCRSHttpService amadeusCRSHttpService,
        ICachingService cachingService,
        ICurrencyRepository currencyRepository,
        IIATACodeRepository iataCodeRepository,
        ILogger<FlightPricingService> logger) : IFlightPricingService
    {
        public async Task<GetFlightPricingResponse> GetFlightPricingAsync(GetFlightPricingRequest request)
        {
            try
            {
                var mappedRequest = request.MapToRequest();

                string cacheKey = mappedRequest.ToString();

                var cachedOffers = cachingService.Get<FlightOfferData>(cacheKey);

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
                var currencies = RetrieveAndAddToCache(nameof(CacheKeys.Currencies), currencyRepository.Get);
                return Task.FromResult(currencies.MapToResponse());
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw new CustomHttpException("An unexpected error has occurred: " + ex.Message);
            }
        }

        public Task<GetIATACodesResponse> GetIATACodes()
        {
            try
            {
                var iataCodes = RetrieveAndAddToCache(nameof(CacheKeys.IATACodes), iataCodeRepository.Get);
                return Task.FromResult(iataCodes.MapToResponse());
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw new CustomHttpException("An unexpected error has occurred: " + ex.Message);
            }
        }

        private IEnumerable<T> RetrieveAndAddToCache<T>(string key, Func<IEnumerable<T>> dataRetrievalCallback)
        {
            var cachedItems = cachingService.Get<T>(key);

            if (cachedItems != null && cachedItems.Any())
            {
                return cachedItems;
            }
            else
            {
                var items = dataRetrievalCallback();

                cachingService.Add(key, items);

                return items;
            }
        }
    }
}
