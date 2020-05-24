using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightControlWeb.DataBaseClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightControlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServersController : ControllerBase
    {
        private ConcurrentDictionary<String, Server> _servers;

        public ServersController(ConcurrentDictionary<String, Server> servers)
        {
            _servers = servers;
        }

        // get /api/servers
        [HttpGet(Name = "GetServers")]
        public ActionResult<List<Server>> GetServers()
        {
            List<Server> serversList = _servers.Values.ToList();
            if (serversList.Count == 0)
            {
                return Ok("no servers");
            }
            return Ok(serversList);
        }

        // post /api/servers
        [HttpPost]
        public async Task<ActionResult> PostServer(Server server)
        {
            if (!_servers.TryAdd(server.ServerId, server))
            {
                return BadRequest("Couldn't add server");
            }

            return CreatedAtAction(actionName: "GetServers", new { id = server.ServerId }, server);
        }

        // delete /api/servers/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteServer(string id)
        {
            if (!_servers.TryRemove(id, out Server server))
            {
                return NotFound(id);
            }
            return Ok(id);
        }
    }
}