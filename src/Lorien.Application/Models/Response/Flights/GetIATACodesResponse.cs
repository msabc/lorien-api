using Lorien.Domain.Models.Data;

namespace Lorien.Application.Models.Response.Flights
{
    public record GetIATACodesResponse
    {
        public required IEnumerable<IATACode> IATACodes { get; set; }
    }
}
