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
        private IDictionary<string, FlightPlan> _flightPlans;
        private IDictionary<string, Server> _servers;
        private IDictionary<string, string> _externalFlights;
        private HttpClient _client;

        public FlightsController(IDictionary<string, FlightPlan> flightPlans,
            IDictionary<string, Server> servers,
            IDictionary<string,string> externalFlights, IHttpClientFactory factory)
        {
            _flightPlans = flightPlans;
            _servers = servers;
            _externalFlights = externalFlights;
            _client = factory.CreateClient("api");
        }

        // get - /api/Flights?relative_to=<DATE_TIME>
        // get - /api/Flights?relative_to=<DATE_TIME> &sync_all
        [HttpGet]
        public async Task<ActionResult<List<Flight>>> GetActiveFlights(string relative_to)
        {

            List<Flight> flights = new List<Flight>();
            DateTime currentTime = Utiles.StringToDateTime(relative_to);
            flights = ActiveFlights(currentTime);
            if (!Request.Query.ContainsKey("sync_all"))
            {
                return Ok(flights);
            }

            var externalFlights = await FlightsFromServers(relative_to);
            flights.AddRange(externalFlights);
            return Ok(flights);
        }
        //delete - /api/Flights/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFlight(string id)
        {
            FlightPlan plan;
            if (!_flightPlans.TryGetValue(id, out plan))
            {
                return NotFound(id);
            }

            _flightPlans.Remove(id);
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
                //get json of flights
                string data = await RetriveDataFromServer(server, relativeTo);
                if (data == "")
                {
                    continue;
                }
                //desrialize
                externalFlights = JsonConvert.DeserializeObject<List<Flight>>(data);
                foreach (Flight flight in externalFlights)
                {
                    bool isOk = _externalFlights.TryAdd(flight.Id, server.ServerId);
                    flight.IsExternal = true;
                }
                //add to flights
                flights.AddRange(externalFlights);
            }
            return flights;
        }

        public async Task<string> RetriveDataFromServer(Server server,string relativeTo)
        {
            HttpResponseMessage respone;
            try
            {
                respone = await _client.GetAsync(server.ServerUrl + "/api/Flights?relative_to=" + relativeTo);
            }
            catch
            {
                return "";
            }
            if (respone.StatusCode != HttpStatusCode.OK)
            {
                return "";
            }
            var content = respone.Content;
            var data = await content.ReadAsStringAsync();
            return data;
        }
    }
}