using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FlightControlWeb.DataBaseClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace FlightControlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightPlanController : ControllerBase
    {
        private ConcurrentDictionary<string,FlightPlan> _flightPlans;
        private ConcurrentDictionary<string, Server> _servers;
        private ConcurrentDictionary<string, string> _externalFlights;
        private HttpClient _client;

        public FlightPlanController(ConcurrentDictionary<string, FlightPlan> flightPlans,
            ConcurrentDictionary<string, Server> servers,
            ConcurrentDictionary<string,string> externalFlights,IHttpClientFactory factory)
        {
            _flightPlans = flightPlans;
            _servers = servers;
            _externalFlights = externalFlights;
            _client = factory.CreateClient("api");
        }
        [HttpPost]
        public ActionResult<FlightPlan> PostFlightPlan([FromBody] FlightPlan plan)
        {
            string id = Utiles.GenarateId(plan.CompanyName);
            bool isAdd = _flightPlans.TryAdd(id, plan);
            if (!isAdd)
            {
                return BadRequest("Error in POST");
            }
            return CreatedAtAction(actionName: "GetFlightPlan", new {id = id}, plan);
        }


        //get - /api/FlightPlan/{id}
        [HttpGet("{id}" , Name = "GetFlightPlan")]
        public async Task<ActionResult<FlightPlan>> GetFlightPlan(string id)
        {
            FlightPlan flight;
            if (_flightPlans.TryGetValue(id, out flight))
            {
                return Ok(flight);
            }
            string serverId;
            if (!_externalFlights.TryGetValue(id, out serverId))
            {
                return NotFound(id);
            }

            Server server;
            if (!_servers.TryGetValue(serverId, out server))
            {
                return NotFound(id);
            }
            //TODO try get from another server
            HttpResponseMessage respone = await _client.GetAsync(server.ServerUrl + "/api/FlightPlan/" + id);
            if (!respone.IsSuccessStatusCode)
            {
                return NotFound(id);
            }
            var content = respone.Content;
            string data = await content.ReadAsStringAsync();
            flight = JsonConvert.DeserializeObject<FlightPlan>(data);

            return Ok(flight);
        }

        // HttpGet without id throws bad request
        [HttpGet]
        public ActionResult<FlightPlan> GetFlightPlanError()
        {
            return BadRequest("No ID");
        }
    }
}