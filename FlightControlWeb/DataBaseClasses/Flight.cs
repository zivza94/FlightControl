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
            Flight_Id = id;
            Longitude = longitude;
            Latitude = latitude;
            Passengers = passengers;
            Company_Name = companyName;
            Date_Time = dateTime;
        }
        public Flight()
        {
        }
        public string Flight_Id { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int Passengers { get; set; }
        public string Company_Name { get; set; }
        public DateTime Date_Time { get; set; }
        public bool Is_External { get; set; }
    }
}
