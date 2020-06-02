using System;
using System.Text.Json.Serialization;
using Newtonsoft.Json;


namespace FlightControlWeb.DataBaseClasses
{
    public class Location
    {
        public Location()
        {
            Longitude = -200;
            Latitude = -200;
            DateTime = DateTime.MinValue;
        }
        public Location(double longitude, double latitude, DateTime time)
        {
            Longitude = longitude;
            Latitude = latitude;
            DateTime= time;

        }
        [JsonProperty("longitude")]
        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("latitude")]
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [Newtonsoft.Json.JsonConverter(typeof(DateFormatConverter), "yyyy-MM-ddTHH:mm:ssZ")]
        [JsonProperty("date_time")]
        [System.Text.Json.Serialization.JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("date_time")]
        public DateTime DateTime { get ; set ; }

        public bool IsValid()
        {
            if (DateTime == DateTime.MinValue)
            {
                return false;
            }

            if (!Utiles.IsLatitudeValid(Latitude) || !Utiles.IsLongitudeValid(Longitude))
            {
                return false;
            }
            return true;
        }
    }
        
}

