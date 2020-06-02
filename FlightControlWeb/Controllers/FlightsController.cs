using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FlightControlWeb.DataBaseClasses;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
//using System.Text.Json.Serialization;


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
        public async Task<ActionResult<List<Flight>>> GetActiveFlights()
        {
            if (!Request.Query.ContainsKey("relative_to"))
            {
                return BadRequest("No relative_to");
            }
            string relativeTo = Request.Query["relative_to"];
            DateTime currentTime = Utiles.StringToDateTime(relativeTo);
            List<Flight> flights = ActiveFlights(currentTime);
            if (!Request.Query.ContainsKey("sync_all"))
            {
                return Ok(flights);
            }

            var externalFlights = await FlightsFromServers(relativeTo);
            flights.AddRange(externalFlights);
            return Ok(flights);
        }
        //delete - /api/Flights/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFlight(string id)
        {
            if (!_flightPlans.TryGetValue(id, out _))
            {
                return NotFound("No flight plan with id: " +id);
            }

            _flightPlans.Remove(id);
            return await Task.FromResult(Ok(id));
        }
        //get the flights from data base
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
        //get flights from external servers
        private async Task<List<Flight>> FlightsFromServers(string relativeTo)
        {
            _externalFlights.Clear();
            List<Flight> flights = new List<Flight>();
            using var client = new HttpClient();
            foreach (Server server in _servers.Values)
            {
                //add to flights
                flights.AddRange(await ExternalFlights(relativeTo, server));
            }
            return flights;
        }

        private async Task<List<Flight>> ExternalFlights(string relativeTo, Server server)
        {
            List<Flight> externalFlights = new List<Flight>();
            //get json of flights
            string data = await RetriveDataFromServer(server, relativeTo);
            if (data == "")
            {
                return externalFlights;
            }

            //desrialize
            externalFlights = JsonConvert.DeserializeObject<List<Flight>>(data);
            List<Flight> tempExternalFlights = new List<Flight>();
            tempExternalFlights.AddRange(externalFlights);
            foreach (Flight flight in tempExternalFlights)
            {
                if (!flight.IsValid())
                {
                    externalFlights.Remove(flight);
                    continue;
                }
                flight.IsExternal = true;
                _externalFlights.TryAdd(flight.Id, server.ServerId);
                
            }
            return externalFlights;
        }

        //connect with external servers
        private async Task<string> RetriveDataFromServer(Server server,string relativeTo)
        {
            HttpResponseMessage respone;
            try
            {
                string requestUri = server.ServerUrl + "/api/Flights?relative_to=" + relativeTo;
                respone = await _client.GetAsync(requestUri);
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