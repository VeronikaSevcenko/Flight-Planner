
namespace FlighttPlanner.Models
{
    public class Flight
    {
        public int Id { get; set; }

        public string Carries { get; set; }

        public string DepartureTime { get; set; }

        public string ArrivalTime { get; set; }

        public Airport From { get; set; }

        public Airport To { get; set; }
    }
}