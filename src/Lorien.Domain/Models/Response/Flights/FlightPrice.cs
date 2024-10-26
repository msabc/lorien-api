namespace Lorien.Domain.Models.Response.Flights
{
    public record FlightPrice
    {
        public required string Currency { get; set; }

        public required string Total { get; set; }

        public required string Base { get; set; }

        public required IEnumerable<FlightFee> Fees { get; set; }

        public required string GrandTotal { get; set; }
    }
}
