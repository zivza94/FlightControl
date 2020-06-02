using System.Text.Json.Serialization;

namespace FlightControlWeb.DataBaseClasses
{
    public class Server
    {
        public Server()
        {
            ServerUrl = null;
            ServerId = null;
        }
        public Server(string id,string url)
        {
            ServerId = id;
            ServerUrl = url;
        }

        [JsonPropertyName("serverId")]
        public string ServerId { get; set; }

        [JsonPropertyName("serverUrl")]
        public string ServerUrl { get; set; }

        public bool ValidateServer()
        {
            if(ServerId ==null || ServerUrl == null)
            {
                return false;
            }

            return true;
        }
        
    }
}
