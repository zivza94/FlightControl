using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.DataBaseClasses
{
    public class Flight
    {
        public Flight(string id, double longitude, double latitude, int passengers, string companyName,
            DateTime dateTime)
        {
            FlightId = id;
            Longitude = longitude;
            Latitude = latitude;
            Passengers = passengers;
            CompanyName = companyName;
            DateTime = dateTime;
        }
        public Flight()
        {
        }
        public string FlightId { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int Passengers { get; set; }
        public string CompanyName { get; set; }
        public DateTime DateTime { get; set; }
        public bool IsExternal { get; set; }
    }
}
