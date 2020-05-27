using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Newtonsoft.Json;


namespace FlightControlWeb.DataBaseClasses
{
    public class FlightPlan
    {
        [JsonPropertyName("company_name")]
        public string CompanyName { get; set; }

        [JsonPropertyName("passengers")]
        public int Passengers { get; set;}

        [JsonPropertyName("initial_location")]
        public Location InitialLocation { get; set; }

        [JsonPropertyName("segments")]
        public LinkedList<Segment> Segments { get; set; }
        public FlightPlan()
        {
            Passengers = -1;
            CompanyName = null;
            InitialLocation = null;
            Segments = null;
        }
        //public string Id => _id;
        public FlightPlan(int passengers, string companyName, Location location, LinkedList<Segment> segments)
        {
            //_id = GenarateID(companyName);
            Passengers = passengers;
            CompanyName = companyName;
            InitialLocation = location;
            Segments = segments;
        }
        public int FindSegment(DateTime currentTime)
        {
            // -1 --> didn't start or already finished
            int seg = -1;
            DateTime startTime = InitialLocation.DateTime;
            while ((startTime <= currentTime) && ((seg+1) < Segments.Count))
            {
                seg++;
                int timespan = Segments.ElementAt(seg).TimespanSecond;
                startTime = startTime.AddSeconds(timespan);
            }

            //seg = seg - 1;
            if (startTime < currentTime || InitialLocation.DateTime > currentTime)
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
            DateTime startsegTime = InitialLocation.DateTime;
            double startLatitude = InitialLocation.Latitude;
            double startLongitude = InitialLocation.Longitude;
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
                startsegTime = startsegTime.AddSeconds(Segments.ElementAt(i).TimespanSecond);
            }
            latitude = Utiles.LinearInterpolation(startLatitude, endLatitude, startsegTime, Segments.ElementAt(i).TimespanSecond, current);
            longitude = Utiles.LinearInterpolation(startLongitude, endLongitude, startsegTime, Segments.ElementAt(i).TimespanSecond, current);
            return new Flight(id,longitude,latitude, Passengers, CompanyName,current);
        }

        public bool IsValid()
        {
            if (CompanyName == null)
            {
                return false;
            }
            if (Segments == null || Segments.Count == 0)
            {
                return false;
            }
            foreach (Segment segment in Segments)
            {
                if (!segment.IsValid())
                {
                    return false;
                }
            }
            
            if (InitialLocation == null || !InitialLocation.IsValid())
            {
                return false;
            }
            if (Passengers < 0)
            {
                return false;
            }
            return true;
        }
    }
}
