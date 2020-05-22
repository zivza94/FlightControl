using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.DataBaseClasses
{
    public class segment
    {
        private double _latitude;
        private double _longitude;
        private int _timespan;

        public segment()
        {
            
        }
        public segment(double startLatitude,double startLongitude,int timespan)
        {
            _latitude = startLatitude;
            _longitude = startLongitude;
            _timespan = timespan;
        }

        public double Latitude { get=>_latitude; set=>_latitude = value; }
        public double Longitude { get=>_longitude; set=>_longitude = value; }
        public int Timespan_seconds
        { 
            get => _timespan;
            set { _timespan = value; }
        }

    }
}
