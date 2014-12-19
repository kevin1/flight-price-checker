using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FlightPriceChecker
{
    class Program
    {
        public static void Main(string[] args)
        {
            var config = JObject.Parse(File.ReadAllText(args[0]));

            decimal threshold = Decimal.Round((decimal)config["search"]["threshold"], decimals: 2);
            var origins = new List<string>();
            foreach (string s in (JArray)config["search"]["origins"]
            ) origins.Add(s);
            var dests = new List<string>();
            foreach (string s in (JArray)config["search"]["destinations"]) dests.Add(s);
            var carriers = new List<string>();
            foreach (string s in (JArray)config["search"]["carriers"]) carriers.Add(s);
            var departDate = DateTime.Parse((string)config["search"]["date"]);

            IEnumerable<Flight> flights = QpxExpress.Search(origins.ToArray(), dests.ToArray(), carriers.ToArray(), departDate)
                .Where(flight => flight.Price < threshold);
            if (flights.Count() == 0) return;

            string msgBody = String.Empty;
            foreach (Flight flight in flights)
            {
                string flightStr = flight.ToString();
                Console.WriteLine(flightStr);
                msgBody += flightStr + "\n";
            }
            msgBody += "\n\nBook at https://www.jetblue.com/";

            try
            {
                var message = new MailMessage

                (
                    from:    (string)config["mailer"]["from"],
                    to:      (string)config["mailer"]["to"],
                    subject: String.Format("Low price found: {0} on {1}", String.Join(", ", dests), departDate.ToShortDateString()),
                    body:    msgBody
                );

                if ((bool)config["mailer"]["acceptAnyCertificate"])
                {
                    ServicePointManager.ServerCertificateValidationCallback = (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true;
                }

                var client = new SmtpClient
                {
                    Host = (string)config["mailer"]["server"],
                    Port = (int)config["mailer"]["port"],
                    UseDefaultCredentials = false,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(userName: (string)config["mailer"]["username"], password: (string)config["mailer"]["password"]),
                    EnableSsl = (bool)config["mailer"]["encryption"],
                };

                client.Send(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(msgBody);
                Console.WriteLine("Encountered exception while emailing flights list: ", e);
            }
        }
    }
}
