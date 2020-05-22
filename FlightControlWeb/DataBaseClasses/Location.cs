using System;


namespace FlightControlWeb.DataBaseClasses
{
    public class Location
    {
        private double _longitude;
        private double _latitude;
        private DateTime _time;

        public Location()
        {
            
        }
        public Location(double longitude, double latitude, DateTime time)
        {
            _longitude = longitude;
            _latitude = latitude;
            _time = time;

        }

        public double Longitude { get=>_longitude; set=>_longitude = value; }
        public double Latitude { get=>_latitude; set=>_latitude = value; }
        public DateTime Date_Time
        {
            get { return _time;}
            set { _time = value; }
        }
    }
        
}

