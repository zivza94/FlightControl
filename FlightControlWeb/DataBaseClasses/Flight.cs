using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FlightControlWeb.DataBaseClasses
{
    public class Flight
    {
        public Flight(string id, double longitude, double latitude, int passengers, string companyName,
            DateTime time)
        {
            Id = id;
            Longitude = longitude;
            Latitude = latitude;
            Passengers = passengers;
            CompanyName = companyName;
            Time = time;
        }
        public Flight()
        {
            
        }
        [JsonProperty("flight_id")]
        [JsonPropertyName("flight_id")]
        public string Id { get; set; }

        [JsonProperty("longitude")]
        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("latitude")]
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("passengers")]
        [JsonPropertyName("passengers")]
        public int Passengers { get; set; }

        [JsonProperty("company_name")]
        [JsonPropertyName("company_name")]
        public string CompanyName { get; set; }

        [Newtonsoft.Json.JsonConverter(typeof(DateFormatConverter), "yyyy-MM-ddTHH:mm:ssZ")]
        [JsonProperty("date_time")]
        [System.Text.Json.Serialization.JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("date_time")]
        public DateTime Time { get; set; }

        [JsonProperty("is_external")]
        [JsonPropertyName("is_external")]
        public bool IsExternal { get; set; }

        public bool IsValid()
        {
            if (Id == null || CompanyName == null || Passengers < 0)
            {
                return false;
            }

            if (!Utiles.IsLongitudeValid(Longitude) || !Utiles.IsLatitudeValid(Latitude))
            {
                return false;
            }

            if (Time == DateTime.MinValue)
            {
                return false;
            }

            return true;
        }
    }
}
