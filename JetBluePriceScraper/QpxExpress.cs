using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JetBluePriceScraper
{
    public static partial class QpxExpress
    {
        static WebClient _client;
        static WebClient Client
        {
            get
            {
                if (_client == null) _client = new WebClient();
                _client.Headers["Content-Type"] = "application/json";
                return _client;
            }
        }

        static string _endpoint;
        static string Endpoint
        {
            get
            {
                if (_endpoint == null)
                {
                    if (String.IsNullOrWhiteSpace(apiKey)) throw new InvalidOperationException("Please specify an API key");
                    _endpoint = String.Format("https://www.googleapis.com/qpxExpress/v1/trips/search?key={0}", apiKey);
                }
                return _endpoint;
            }
        }

        public static IEnumerable<Flight> Search(string[] origins, string[] destinations, string[] airlines, DateTime departDate)
        {
            foreach (string origin in origins)
            {
                foreach (string dest in destinations)
                {
                    IEnumerable<Flight> prices = Search(origin, dest, airlines, departDate);
                    foreach (Flight price in prices) yield return price;
                }
            }
        }

        public static IEnumerable<Flight> Search(string origin, string dest, string[] airlines, DateTime departDate)
        {
            JObject requestJson = CreateBaseRequest();
            requestJson["request"]["passengers"]["adultCount"] = 1;

            var slice = (JObject)requestJson["request"]["slice"][0];
            slice["origin"] = origin;
            slice["destination"] = dest;
            slice["date"] = departDate.ToString("yyyy-MM-dd");
            slice["permittedCarrier"] = new JArray(airlines);

            JObject response = JObject.Parse(Client.UploadString(Endpoint, requestJson.ToString()));
            var options = (JArray)response["trips"]["tripOption"];
            foreach (JObject option in options)
            {
                var retVal = new Flight();

                string priceStr = (string)option["saleTotal"]; // expecting something like "USD300.10"
                // Only use digit characters, period, and comma
                priceStr = new string((from c in priceStr where Char.IsDigit(c) || c == '.' || c == ',' select c).ToArray());
                retVal.Price = Decimal.Parse(priceStr);

                JObject flightLeg = (JObject)option["slice"][0]["segment"][0]["leg"][0];

                retVal.Origin = (string)flightLeg["origin"];
                retVal.Destination = (string)flightLeg["destination"];
                retVal.TimeDepart = DateTimeOffset.Parse((string)flightLeg["departureTime"]);
                retVal.TimeArrive = DateTimeOffset.Parse((string)flightLeg["arrivalTime"]);

                yield return retVal;
            }
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
                                    new JProperty("adultCount", 0),
                                    new JProperty("childCount", 0),
                                    new JProperty("infantInLapCount", 0),
                                    new JProperty("infantInSeatCount", 0),
                                    new JProperty("seniorCount", 0)
                                )
                            ),
                            new JProperty
                            (
                                "slice",
                                new JArray(
                                    new JObject(
                                        new JProperty("origin", String.Empty),
                                        new JProperty("destination", String.Empty),
                                        new JProperty("date", String.Empty),
                                        new JProperty("maxStops", 0),
                                        new JProperty("preferredCabin", "COACH"),
                                        new JProperty("permittedCarrier", new JArray(String.Empty))
                                    )
                                )
                            ),
                            new JProperty("saleCountry", "US"),
                            new JProperty("refundable", false),
                            new JProperty("solutions", 500)
                        )
                    )
                );
            return retVal;
        }
    }

    public class Flight
    {
        public string Origin, Destination;
        public decimal Price;
        public DateTimeOffset TimeDepart, TimeArrive;

        public override string ToString()
        {

            return String.Format("{0} ({1}) -> {2} ({3}) for {4:C}", Origin, TimeDepart, Destination, TimeArrive, Price);
        }
    }
}

