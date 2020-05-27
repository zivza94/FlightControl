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
        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

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

