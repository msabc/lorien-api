namespace Lorien.Domain.Models.Response.Flights
{
    public class FlightEndpoint
    {
        public required string IATACode { get; set; }

        public required string Terminal { get; set; }

        public DateTime At { get; set; }
    }
}
