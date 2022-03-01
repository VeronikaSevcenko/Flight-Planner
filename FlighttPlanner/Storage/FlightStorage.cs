using FlighttPlanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
namespace FlighttPlanner.Storage
{
    public class FlightStorage
    {
        private static readonly object _a = new();

        private static List<Flight> _flights = new();

        private static int _id = 0;

        public static Flight AddFlight(AddFlightRequest request)
        {
            lock (_a)
            {
                var flight = new Flight
                {
                    ArrivalTime = request.ArrivalTime,
                    DepartureTime = request.DepartureTime,
                    Carries = request.Carries,
                    From = request.From,
                    To = request.To,
                    Id = ++_id
                };
                _flights.Add(flight);

                return flight;
            }
        }

        public static Flight GetFlight(int id)
        {
            lock (_a)
            {
                return _flights.SingleOrDefault(f => f.Id == id);
            }
        }

     
        public static void Clear()
        {
            lock (_a)
            {
                _id = 0;
                _flights.Clear();
            }
        }

        public static bool Exists(AddFlightRequest request)
        {
            lock (_a)
            {
                return _flights.Any(f =>

                 f.Carries.ToLower().Trim() == request.Carries.ToLower().Trim() &&
                 f.DepartureTime.ToLower().Trim() == request.DepartureTime.ToLower().Trim() &&
                 f.ArrivalTime.ToLower().Trim() == request.ArrivalTime.ToLower().Trim() &&
                 f.From.AirportName.ToLower().Trim() == request.From.AirportName.ToLower().Trim() &&
                 f.To.AirportName.ToLower().Trim() == request.To.AirportName.ToLower().Trim());
            }
        }

        public static List<Airport> FindAirports(string userInput)
        {
            lock (_a)
            {
                var fromAirport = _flights.Where(a =>

                   a.From.AirportName.ToLower().Trim().Contains(userInput.ToLower().Trim()) ||
                   a.From.Country.ToLower().Trim().Contains(userInput.ToLower().Trim()) ||
                   a.From.City.ToLower().Trim().Contains(userInput)).Select(a => a.From).ToList();

                var toAirport = _flights.Where(a =>

                   a.To.AirportName.ToLower().Trim().Contains(userInput.ToLower().Trim()) ||
                   a.To.Country.ToLower().Trim().Contains(userInput) ||
                   a.To.City.ToLower().Trim().Contains(userInput.ToLower().Trim())).Select(a => a.To).ToList();

                return fromAirport.Concat(toAirport).ToList();
            }
        }

        public static PageResult SearchFlight(SearchFlightRequest req)
        {
            lock (_a)
            {
                return new PageResult(_flights);
            }
        }

        public static void DeleteFlight(int id)
        {
            lock (_a)
            {
                var flight = GetFlight(id);

                if (flight != null)
                    _flights.Remove(flight);
            }
        }

        public static bool IsValid(AddFlightRequest request)
        {
            lock (_a)
            {
                if (request == null)
                    return false;

                if (string.IsNullOrEmpty(request.ArrivalTime) || string.IsNullOrEmpty(request.DepartureTime) || string.IsNullOrEmpty(request.Carries))
                    return false;

                if (request.From == null || request.To == null)
                    return false;

                if (string.IsNullOrEmpty(request.From.AirportName) || string.IsNullOrEmpty(request.From.City) ||
                    string.IsNullOrEmpty(request.From.Country) || string.IsNullOrEmpty(request.To.AirportName) ||
                    string.IsNullOrEmpty(request.To.City) || string.IsNullOrEmpty(request.To.Country))
                    return false;

                if (request.From.Country.ToLower().Trim() == request.To.Country.ToLower().Trim() &&
                    request.From.City.ToLower().Trim() == request.To.City.ToLower().Trim() &&
                    request.From.AirportName.ToLower().Trim() == request.To.AirportName.ToLower().Trim())
                    return false;

                var arrivalTime = DateTime.Parse(request.ArrivalTime);
                var departureTime = DateTime.Parse(request.DepartureTime);

                if (arrivalTime <= departureTime)
                    return false;

                return true;
            }
        }

        public static bool IsValidFlight(SearchFlightRequest request)
        {
            lock (_a)
            {
                if (string.IsNullOrEmpty(request.To) || string.IsNullOrEmpty(request.From) || string.IsNullOrEmpty(request.DepartureDate))
                    return false;

                if (request.From == request.To)
                    return false;

                return true;
            }
        }
    }
}
