using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace FlightPriceChecker
{
    class Program
    {
        public static void Main(string[] args)
        {
            IEnumerable<Flight> flights = QpxExpress.Search(new[] { "OAK", "SJC", "SFO" }, new[] { "NYC" }, new[] { "B6" }, new DateTime(year: 2015, month: 1, day: 8));
            foreach (Flight flight in flights)
            {
                Console.WriteLine(flight.ToString());
            }
        }
    }
}
