namespace Lorien.Domain.Models.Response.Flights
{
    public record FlightFee
    {
        public required string Amount { get; set; }

        public required string Type { get; set; }
    }
}
