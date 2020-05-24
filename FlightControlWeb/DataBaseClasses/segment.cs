namespace FlightControlWeb.DataBaseClasses
{
    public class Segment
    {
        public Segment()
        {}
        public Segment(double startLatitude,double startLongitude,int timespan)
        {
            Latitude= startLatitude;
            Longitude = startLongitude;
            TimespanSeconds = timespan;
        }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int TimespanSeconds { get; set; }

    }
}
