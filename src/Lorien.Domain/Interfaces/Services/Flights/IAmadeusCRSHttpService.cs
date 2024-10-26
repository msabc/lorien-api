using Lorien.Domain.Models.Request;
using Lorien.Domain.Models.Response.Flights;

namespace Lorien.Domain.Interfaces.Services.Flights
{
    public interface IAmadeusCRSHttpService
    {
        Task<GetFlightOfferResponse> GetFlightOffers(GetFlightOfferRequest request);
    }
}
