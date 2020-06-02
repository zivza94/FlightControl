using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightControlWeb.DataBaseClasses;
using Microsoft.AspNetCore.Mvc;

namespace FlightControlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServersController : ControllerBase
    {
        private IDictionary<String, Server> _servers;

        public ServersController(IDictionary<String, Server> servers)
        {
            _servers = servers;
        }

        // get /api/servers
        [HttpGet(Name = "GetServers")]
        public async Task<ActionResult<List<Server>>> GetServers()
        {
            List<Server> serversList = _servers.Values.ToList();
            return await Task.FromResult(Ok(serversList));
        }

        // post /api/servers
        [HttpPost]
        public async Task<ActionResult> PostServer(Server server)
        {
            if (!server.ValidateServer())
            {
                return await Task.FromResult(BadRequest("server not valid"));
            }
            if (!_servers.TryAdd(server.ServerId, server))
            {
                return await Task.FromResult(BadRequest("Couldn't add server"));
            }
            return await Task.FromResult(
                CreatedAtAction(actionName: "GetServers", new { id = server.ServerId }, server));
        }

        // delete /api/servers/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteServer(string id)
        {
            if (!_servers.TryGetValue(id, out _))
            {
                return await Task.FromResult(NotFound("No Server with ID: " + id));
            }
            _servers.Remove(id);
            return await Task.FromResult(Ok(id));
        }
    }
}