using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.DataBaseClasses
{
    public class FlightPlan
    {
        //private string _id;
        private int _passengers;
        private string _companyName;
        private Location _location;
        private LinkedList<segment> _segments;

        public string Company_Name { get => _companyName;
            set => _companyName = value;
        }
        public int Passengers { get =>_passengers; set =>_passengers = value; }
        public Location Initial_location { get => _location; set => _location = value; }
        public LinkedList<segment> Segments { get=>_segments; set=>_segments = value; }
        public FlightPlan()
        {
            
        }
        //public string Id => _id;
        public FlightPlan(int passengers, string companyName, Location location, LinkedList<segment> segments)
        {
            //_id = GenarateID(companyName);
            _passengers = passengers;
            _companyName = companyName;
            _location = location;
            _segments = segments;
        }
        public int FindSegment(DateTime currentTime)
        {
            // -1 --> didn't start or already finished
            int seg = -1;
            DateTime startTime = _location.Date_Time;
            while ((startTime <= currentTime) && ((seg+1) < Segments.Count))
            {
                seg++;
                int timespan = _segments.ElementAt(seg).Timespan_seconds;
                startTime = startTime.AddSeconds(timespan);
            }

            //seg = seg - 1;
            if (startTime < currentTime || _location.Date_Time > currentTime)
            {
                seg = -1;
            }
            return seg;
        }
        public Flight GetFlightByTime(DateTime current,string id)
        {
            double latitude, longitude;
            int segIndex = FindSegment(current);
            if (segIndex == -1)
            {
                return null;
            }
            DateTime startsegTime = _location.Date_Time;
            double startLatitude = _location.Latitude;
            double startLongitude = _location.Longitude;
            double endLatitude = _segments.ElementAt(segIndex).Latitude;
            double endLongitude = _segments.ElementAt(segIndex).Longitude;
            if (segIndex != 0)
            {
                startLatitude = _segments.ElementAt(segIndex - 1).Latitude;
                startLongitude = _segments.ElementAt(segIndex - 1).Longitude;
            }
            int i = 0;
            while (i < segIndex)
            {
                i++;
                startsegTime = startsegTime.AddSeconds(_segments.ElementAt(i).Timespan_seconds);
            }
            latitude = Utiles.LinearInterpolation(startLatitude, endLatitude, startsegTime, _segments.ElementAt(i).Timespan_seconds, current);
            longitude = Utiles.LinearInterpolation(startLongitude, endLongitude, startsegTime, _segments.ElementAt(i).Timespan_seconds, current);
            return new Flight(id,longitude,latitude, _passengers, _companyName,current);
        }
    }
}
