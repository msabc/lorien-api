using Lorien.Application.Models.Request;
using Lorien.Application.Models.Response.Flights;

namespace Lorien.Application.Services
{
    public interface IFlightPricingService
    {
        Task<GetFlightPricingResponse> GetFlightPricingAsync(GetFlightPricingRequest request);

        Task<GetCurrenciesResponse> GetCurrencies();

        Task<GetIATACodesResponse> GetIATACodes();
    }
}
