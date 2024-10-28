using Lorien.Domain.Models.Data;

namespace Lorien.Application.Models.Response.Flights
{
    public record GetCurrenciesResponse
    {
        public required IEnumerable<Currency> Currencies { get; set; }
    }
}
