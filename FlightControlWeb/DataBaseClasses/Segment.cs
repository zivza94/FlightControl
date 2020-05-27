using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
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

        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }
        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

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
