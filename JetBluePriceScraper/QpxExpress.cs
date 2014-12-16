using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JetBluePriceScraper
{
    public static partial class QpxExpress
    {
        public static FlightPrice[] Search(string[] origins, string[] destinations, string[] airlines, DateTime departDate)
        {
            return null;
        }

        public static JObject CreateBaseRequest()
        {
            var retVal = new JObject
                (
                    new JProperty
                    (
                        "request",
                        new JObject
                        (
                            new JProperty
                            (
                                "passengers",
                                new JObject
                                (
                                    new JProperty("adultCount", 1),
                                    new JProperty("childCount", 0),
                                    new JProperty("infantInLapCount", 0),
                                    new JProperty("seniorCount", 0)
                                )
                            ),
                            new JProperty
                            (
                                "slice",
                                new JArray(
                                    new JObject(
                                        new JProperty("origin", "SFO"),
                                        new JProperty("destination", "JFK"),
                                        new JProperty("date", "2015-01-08"),
                                        new JProperty("maxStops", 0),
                                        new JProperty("preferredCabin", "COACH"),
                                        new JProperty("permittedCarrier", new JArray("B6"))
                                    )
                                )
                            ),
                            new JProperty("saleCountry", "US"),
                            new JProperty("refundable", false),
                            new JProperty("solutions", 500)
                        )
                    )
                );
            Console.WriteLine(retVal.ToString());
            return retVal;
        }

        public class FlightPrice
        {
            public string Origin, Destination;
            public decimal Price;
            public DateTime TimeDepart, TimeArrive;
        }
    }
}

