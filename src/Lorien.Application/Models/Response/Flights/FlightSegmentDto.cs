namespace Lorien.Application.Models.Response.Flights
{
    public record FlightSegmentDto
    {
        public required FlightDepartureAndArrivalDto Departure { get; set; }

        public required FlightDepartureAndArrivalDto Arrival { get; set; }

        public required string CarrierCode { get; set; }

        public required string Number { get; set; }

        public required string Duration { get; set; }

        public required string Id { get; set; }

        public required int NumberOfStops { get; set; }
    }
}
