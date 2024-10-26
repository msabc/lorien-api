namespace Lorien.Domain.Models.Response.Flights
{
    public record FlightSegment
    {
        public required FlightDepartureAndArrival Departure {get;set;}

        public required FlightDepartureAndArrival Arrival {get;set; }

        public required string CarrierCode { get; set; }

        public required string Number { get; set; }

        public required string Duration { get; set; }

        public required string Id { get; set; }

        public required int NumberOfStops { get; set; }
    }
}
