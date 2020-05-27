using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

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
        public DateTime Time { get; set; }
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
