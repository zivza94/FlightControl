using System;


namespace FlightControlWeb.DataBaseClasses
{
    public class Location
    {
        public Location()
        {
            
        }
        public Location(double longitude, double latitude, DateTime time)
        {
            Longitude = longitude;
            Latitude = latitude;
            Date_Time= time;

        }

        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public DateTime Date_Time { get ; set ; }
    }
        
}

