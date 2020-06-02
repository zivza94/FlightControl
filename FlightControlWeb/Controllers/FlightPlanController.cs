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
using System.Text.Json;
using Newtonsoft.Json;

//using Newtonsoft.Json;

namespace FlightControlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightPlanController : ControllerBase
    {
        private IDictionary<string,FlightPlan> _flightPlans;
        private IDictionary<string, Server> _servers;
        private IDictionary<string, string> _externalFlights;
        private HttpClient _client;

        public FlightPlanController(IDictionary<string, FlightPlan> flightPlans,
            IDictionary<string, Server> servers,
            IDictionary<string,string> externalFlights,IHttpClientFactory factory)
        {
            _flightPlans = flightPlans;
            _servers = servers;
            _externalFlights = externalFlights;
            _client = factory.CreateClient("api");
        }
        [HttpPost]
        public async Task<ActionResult<FlightPlan>> PostFlightPlan(FlightPlan plan)
        {
            //FlightPlan plan = JsonConvert.DeserializeObject<FlightPlan>(planJson);
            if (!plan.IsValid())
            {
                return BadRequest("Flight plan isn't valid, couldn't post");
            }
            string id = Utiles.GenerateId(plan.CompanyName);
            bool isAdd = _flightPlans.TryAdd(id, plan);
            if (!isAdd)
            {
                return BadRequest("Error in POST flight lan");
            }

            var retval = CreatedAtAction(actionName: "GetFlightPlan", new {id}, plan);
            return await Task.FromResult(retval);
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
                return NotFound("No flight plan with id: "+ id);
            }

            Server server;
            if (!_servers.TryGetValue(serverId, out server))
            {
                return NotFound("No flight plan with id: " + id);
            }

            string requestUri = server.ServerUrl + "/api/FlightPlan/" + id;
            HttpResponseMessage respone = await _client.GetAsync(requestUri);
            if (!respone.IsSuccessStatusCode)
            {
                return NotFound("No flight plan with id: " + id);
            }
            var content = respone.Content;
            string data = await content.ReadAsStringAsync();
            flight = JsonConvert.DeserializeObject<FlightPlan>(data);

            return Ok(flight);
        }

        // HttpGet without id throws bad request
        [HttpGet]
        public async Task<ActionResult<FlightPlan>> GetFlightPlanError()
        {
            return await Task.FromResult(BadRequest("Please add ID"));
        }


        
    }
}