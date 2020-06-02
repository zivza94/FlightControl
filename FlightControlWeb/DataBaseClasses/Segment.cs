using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace FlightControlWeb.DataBaseClasses
{
    public class Segment
    {

        public Segment()
        {
            Latitude = -200;
            Longitude = -200;
            TimespanSecond = -1;
        }
        public Segment(double startLatitude, double startLongitude, int timespan)
        {
            Latitude = startLatitude;
            Longitude = startLongitude;
            TimespanSecond = timespan;
        }
        [JsonProperty("latitude")]
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("timespan_seconds")]
        [JsonPropertyName("timespan_seconds")]
        public int TimespanSecond { get; set; }

        public bool IsValid()
        {
            if (!Utiles.IsLongitudeValid(Longitude) || !Utiles.IsLatitudeValid(Latitude))
            {
                return false;
            }

            if (TimespanSecond < 0)
            {
                return false;
            }
            return true;
        }
    }
}
