using System.Collections.Generic;

namespace FlighttPlanner.Models
{
    public class PageResult
    {
        public int Page { get; set; }

        public int TotalItems { get; set; }

        public List<Flight> Items { get; set; }

        public PageResult(List<Flight> items)
        {
            TotalItems = items.Count;
            Items = items;
            Page = 0;
        }
    }
}
