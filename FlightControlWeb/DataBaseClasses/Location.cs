using System;
using System.Text.Json.Serialization;
using Newtonsoft.Json;


namespace FlightControlWeb.DataBaseClasses
{
    public class Location
    {
        public Location()
        {
            
        }
        public Location(double longitude, double latitude, DateTime time)
        {
            Longitude = longitude;
            Latitude = latitude;
            DateTime= time;

        }
        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("date_time")]
        public DateTime DateTime { get ; set ; }

        public bool ValidateLocation()
        {
            if (Longitude != null && Latitude != null && DateTime != null)
            {
                return true;
            }

            return false;
        }
    }
        
}

