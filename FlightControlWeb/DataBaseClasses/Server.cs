namespace FlightControlWeb.DataBaseClasses
{
    public class Server
    {
        public Server()
        {

        }
        public Server(string id,string url)
        {
            ServerId = id;
            ServerUrl = url;
        }

        public string ServerId { get; set; }
        public string ServerUrl { get; set; }
        
    }
}
