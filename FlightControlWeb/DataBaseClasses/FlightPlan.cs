using System;
using System.Collections.Generic;
using System.Linq;


namespace FlightControlWeb.DataBaseClasses
{
    public class FlightPlan
    {
        public string Company_Name { get; set; }
        public int Passengers { get; set;}
        public Location Initial_Location { get; set; }
        public LinkedList<Segment> Segments { get; set; }
        public FlightPlan()
        {
            
        }
        //public string Id => _id;
        public FlightPlan(int passengers, string companyName, Location location, LinkedList<Segment> segments)
        {
            //_id = GenarateID(companyName);
            Passengers = passengers;
            Company_Name = companyName;
            Initial_Location = location;
            Segments = segments;
        }
        public int FindSegment(DateTime currentTime)
        {
            // -1 --> didn't start or already finished
            int seg = -1;
            DateTime startTime = Initial_Location.Date_Time;
            while ((startTime <= currentTime) && ((seg+1) < Segments.Count))
            {
                seg++;
                int timespan = Segments.ElementAt(seg).Timespan_Seconds;
                startTime = startTime.AddSeconds(timespan);
            }

            //seg = seg - 1;
            if (startTime < currentTime || Initial_Location.Date_Time > currentTime)
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
            DateTime startsegTime = Initial_Location.Date_Time;
            double startLatitude = Initial_Location.Latitude;
            double startLongitude = Initial_Location.Longitude;
            double endLatitude = Segments.ElementAt(segIndex).Latitude;
            double endLongitude = Segments.ElementAt(segIndex).Longitude;
            if (segIndex != 0)
            {
                startLatitude = Segments.ElementAt(segIndex - 1).Latitude;
                startLongitude = Segments.ElementAt(segIndex - 1).Longitude;
            }
            int i = 0;
            while (i < segIndex)
            {
                i++;
                startsegTime = startsegTime.AddSeconds(Segments.ElementAt(i).Timespan_Seconds);
            }
            latitude = Utiles.LinearInterpolation(startLatitude, endLatitude, startsegTime, Segments.ElementAt(i).Timespan_Seconds, current);
            longitude = Utiles.LinearInterpolation(startLongitude, endLongitude, startsegTime, Segments.ElementAt(i).Timespan_Seconds, current);
            return new Flight(id,longitude,latitude, Passengers, Company_Name,current);
        }
    }
}
