using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.DataBaseClasses
{
    public class Flight
    {
        private string _id;
        private double _longitude;
        private double _latitude;
        private int _passangers;
        private string _companyName;
        private DateTime _dateTime;
        private bool _isExternal;


        public Flight(string id, double longitude, double latitude, int passengers, string companyName,
            DateTime dateTime)
        {
            _id = id;
            _longitude = longitude;
            _latitude = latitude;
            _passangers = passengers;
            _companyName = companyName;
            _dateTime = dateTime;
        }

        public bool Is_external { get=>_isExternal; set=>_isExternal = value; }
        public string Flight_id { get =>_id;}
        public double Latitude { get => _latitude;}
        public double Longitude { get =>_longitude;}
        public int Passengers { get =>_passangers;}
        public DateTime Date_Time { get=>_dateTime;}
        public string Company_name { get => _companyName;}
    }
}
