using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FlightControlWeb.DataBaseClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightControlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private ConcurrentDictionary<string, FlightPlan> _flightPlans;
        private ConcurrentDictionary<string, Server> _servers;
        private ConcurrentDictionary<string, string> _externalFlights;
        private HttpClient _client;

        public FlightsController(ConcurrentDictionary<string, FlightPlan> flightPlans,
            ConcurrentDictionary<string, Server> servers,
            ConcurrentDictionary<string,string> externalFlights, IHttpClientFactory factory)
        {
            _flightPlans = flightPlans;
            _servers = servers;
            _externalFlights = externalFlights;
            _client = factory.CreateClient("api");
        }

        // get - /api/Flights?relative_to=<DATE_TIME>
        // get - /api/Flights?relative_to=<DATE_TIME> &sync_all
        [HttpGet]
        public ActionResult<List<Flight>> GetActiveFlights(string relative_to)
        {

            List<Flight> flights = new List<Flight>();
            DateTime currentTime = Utiles.StringToDateTime(relative_to);
            flights = activeFlights(currentTime);
            if (!Request.Query.ContainsKey("sync_all"))
            {
                return Ok(flights);
            }
            flights.AddRange(FlightsFromServers(relative_to).Result);
            return Ok(flights);
        }
        //delete - /api/Flights/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteFlight(string id)
        {
            FlightPlan plan;
            if (!_flightPlans.TryRemove(id, out plan))
            {
                return NotFound(id);
            }

            return Ok(id);
        }

        private List<Flight> activeFlights(DateTime currentTime)
        {
            List<Flight> flights = new List<Flight>();
            foreach (var entry in _flightPlans)
            {
                Flight flight = entry.Value.GetFlightByTime(currentTime,entry.Key);
                if (flight != null)
                {
                    flight.Is_external= false;
                    flights.Add(flight);
                }
            }

            return flights;
        }

        private async Task<List<Flight>> FlightsFromServers(string relative_to)
        {
            List<Flight> flights = new List<Flight>();
            using var client = new HttpClient();
            foreach (Server server in _servers.Values)
            {
                List<Flight> externalFlights = new List<Flight>();
                //TODO ask for other servers for Flights
                HttpResponseMessage respone =  await _client.GetAsync(server.ServerUrl + "/api/Flights?relative_to=" + relative_to);
                foreach (Flight flight in externalFlights)
                {
                    bool isOk = _externalFlights.TryAdd(flight.Flight_id, server.ServerId);
                    flight.Is_external = true;
                }
                flights.AddRange(externalFlights);
            }

            return flights;
        }
    }
}