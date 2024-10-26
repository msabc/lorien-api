using Lorien.Application.Models.Request;
using Lorien.Application.Models.Response.Flights;
using Lorien.Domain.Models.Request;
using Lorien.Domain.Models.Response.Flights;

namespace Lorien.Application.Mappers
{
    public static class FlightPricingMapper
    {
        public static GetFlightOfferRequest MapToRequest(this GetFlightPricingRequest request)
        {
            return new GetFlightOfferRequest()
            {
                DepartureDate = request.DepartureDate,
                DestinationLocationCode = request.DestinationLocationCode,
                OriginLocationCode = request.OriginLocationCode,
                Adults = request.NumberOfPassengers
            };
        }

        public static GetFlightPricingResponse MapToResponse(this GetFlightOfferResponse response)
        {
            return new GetFlightPricingResponse()
            {
                FlightOffers = response.Data.Select(x => x.MapToDto())
            };
        }

        private static FlightOfferDto MapToDto(this FlightOfferData response)
        {
            return new FlightOfferDto()
            {
                Id = response.Id,
                Type = response.Type,
                Source = response.Source,
                OneWay = response.OneWay,
                NumberOfBookableSeats = response.NumberOfBookableSeats,
                Currency = response.Price.Currency,
                GrandTotal = response.Price.GrandTotal,
                Itineraries = response.Itineraries.Select(x => x.MapToDto())
            };
        }

        private static FlightItineraryDto MapToDto(this FlightItinerary itinerary)
        {
            return new FlightItineraryDto()
            {
                Duration = itinerary.Duration,
                Segments = itinerary.Segments.Select(x => x.MapToDto())
            };
        }

        private static FlightSegmentDto MapToDto(this FlightSegment segment)
        {
            return new FlightSegmentDto()
            {
                Id = segment.Id,
                CarrierCode = segment.CarrierCode,
                Duration = segment.Duration,
                Number = segment.Number,
                NumberOfStops = segment.NumberOfStops,
                Arrival = new FlightDepartureAndArrivalDto()
                {
                    At = segment.Arrival.At,
                    IATACode = segment.Arrival.IATACode,
                    Terminal = segment.Arrival.Terminal,
                },
                Departure = new FlightDepartureAndArrivalDto()
                {
                    At = segment.Departure.At,
                    IATACode = segment.Departure.IATACode,
                    Terminal = segment.Departure.Terminal,
                }
            };
        }
    }
}
