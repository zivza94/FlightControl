using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FlightControlWeb.DataBaseClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


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
        public ActionResult<List<Flight>> GetActiveFlights(string relativeTo)
        {

            List<Flight> flights = new List<Flight>();
            DateTime currentTime = Utiles.StringToDateTime(relativeTo);
            flights = ActiveFlights(currentTime);
            if (!Request.Query.ContainsKey("sync_all"))
            {
                return Ok(flights);
            }
            flights.AddRange(FlightsFromServers(relativeTo).Result);
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

        private List<Flight> ActiveFlights(DateTime currentTime)
        {
            List<Flight> flights = new List<Flight>();
            foreach (var entry in _flightPlans)
            {
                Flight flight = entry.Value.GetFlightByTime(currentTime,entry.Key);
                if (flight != null)
                {
                    flight.IsExternal= false;
                    flights.Add(flight);
                }
            }

            return flights;
        }

        private async Task<List<Flight>> FlightsFromServers(string relativeTo)
        {
            _externalFlights.Clear();
            List<Flight> flights = new List<Flight>();
            using var client = new HttpClient();
            foreach (Server server in _servers.Values)
            {
                List<Flight> externalFlights = new List<Flight>();
                //TODO ask for other servers for Flights
                HttpResponseMessage respone =  await _client.GetAsync(server.ServerUrl + "/api/Flights?relative_to=" + relativeTo);
                if (respone.StatusCode != HttpStatusCode.OK)
                {
                    return null;
                }

                var content = respone.Content;
                var data = await content.ReadAsStringAsync();
                externalFlights = JsonConvert.DeserializeObject<List<Flight>>(data);
                foreach (Flight flight in externalFlights)
                {
                    bool isOk = _externalFlights.TryAdd(flight.FlightId, server.ServerId);
                    flight.IsExternal = true;
                }
                flights.AddRange(externalFlights);
            }

            return flights;
        }
    }
}