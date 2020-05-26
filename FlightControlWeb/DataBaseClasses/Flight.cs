using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FlightControlWeb.DataBaseClasses
{
    public class Flight
    {
        public Flight(string id, double longitude, double latitude, int passengers, string companyName,
            DateTime dateTime)
        {
            Id = id;
            Longitude = longitude;
            Latitude = latitude;
            Passengers = passengers;
            CompanyName = companyName;
            DateTime = dateTime;
        }
        public Flight()
        {
        }
        [JsonPropertyName("flight_id")]
        public string Id { get; set; }
        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }
        [JsonPropertyName("passengers")]
        public int Passengers { get; set; }
        [JsonPropertyName("company_name")]
        public string CompanyName { get; set; }
        [JsonPropertyName("date_time")]
        public DateTime DateTime { get; set; }
        [JsonPropertyName("is_external")]
        public bool IsExternal { get; set; }
    }
}
