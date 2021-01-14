using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using aspnetcore_webapp.Entities;
using Npgsql;

namespace aspnetcore_webapp.Pages
{
    public class IndexModel : PageModel
    {
        public IList<Airport> airports;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            this.airports = GetAirports();
        }

        private IList<Airport> GetAirports()
        {
            IList<Airport> airports = new List<Airport>();
            var connString = "Host=localhost;Username=postgres;Password=newton243;Database=postgres";

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand("SELECT * FROM flightnet_airports", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Airport myAirport = new Airport();
                            myAirport.airport = (string)reader["airport"];
                            myAirport.city = (string)reader["city"];
                            myAirport.country = (string)reader["country"];
                            myAirport.iata_code = (string)reader["iata_code"];
                            myAirport.latitude = 0;
                            myAirport.longitude = 0;
                            myAirport.state = (string)reader["state"];
                            airports.Add(myAirport);
                        }
                    }
                }
            }
            return airports;
        }
    }
}
