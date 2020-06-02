using System;
using System.Linq;


namespace FlightControlWeb
{
    public static class Utiles
    {
        public static string DateTimeToString(DateTime time)
        {
            string retval;
            string datetime = time.ToString("yyyy-MM-ddTHH:mm:ssZ");
            //retval = time.Year + "-" + time.Month + "-" + time.Day + "T" + time.TimeOfDay + 'Z';
            return datetime;
        }

        public static DateTime StringToDateTime(string time)
        {
            if (time[0] == '<')
            {
                time = time.Substring(1, time.Length - 2);
            }
            string[] timeArr = time.Split(':', '-', 'T','Z');
            int year = int.Parse(timeArr[0]);
            int mounth = int.Parse(timeArr[1]);
            int day = int.Parse(timeArr[2]);
            int hours = int.Parse(timeArr[3]);
            int min = int.Parse(timeArr[4]);
            int sec = int.Parse(timeArr[5]);
            //int timezone = int.Parse(timeArr[5].Substring(2));

            return new DateTime(year, mounth, day, hours, min, sec);
        }
        public static double LinearInterpolation(double start, double end, DateTime startTime, int timespan, DateTime time)
        {
            double current = start;
            double length = end - start;
            int currentSec = (int)time.Subtract(startTime).TotalSeconds;
            //add the logic of the interpolation
            double timePass = (double)currentSec / (double)timespan;
            current += timePass * length;
            return current;
        }

        public static string GenerateId(string start)
        {
            string id = "";
            if (start.Length < 2)
            {
                Random rnd = new Random();
                id += (char)rnd.Next('A', 'Z');
                id += (char)rnd.Next('A', 'Z');
            }
            else
            {
                id += start.Substring(0, 2).ToUpper();
            }
            id += new Random().Next(1000, 9999);
            return id;
        }

        public static bool IsLongitudeValid(double longitude)
        {
            if (longitude < -180 || longitude > 180)
            {
                return false;
            }
            return true;
        }
        public static bool IsLatitudeValid(double latitude)
        {
            if (latitude < -90 || latitude > 90)
            {
                return false;
            }
            return true;
        }
    }
}
