using System;
using System.Linq;


namespace FlightControlWeb
{
    public static class Utiles
    {
        public static string DateTimeToString(DateTime time)
        {
            string retval;
            retval = time.Year + "-" + time.Month + "-" + time.Day + "T" + time.TimeOfDay + 0;
            return retval;
        }

        public static DateTime StringToDateTime(string time)
        {
            if (time[0] == '<')
            {
                time = time.Substring(1, time.Length - 2);
            }
            string[] timeArr = time.Split(':', '-', 'T');
            int year = int.Parse(timeArr[0]);
            int mounth = int.Parse(timeArr[1]);
            int day = int.Parse(timeArr[2]);
            int hours = int.Parse(timeArr[3]);
            int min = int.Parse(timeArr[4]);
            int sec = int.Parse(timeArr[5].Remove(2));
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

        public static string GenarateId(string start)
        {
            string id = "";
            id += start.Substring(0, 2);
            id += new Random().Next(1000, 9999);
            return id;
        }

        public static bool IsLongitudeValid(double longitude)
        {
            if (longitude <= -200)
            {
                return false;
            }
            return true;
        }
        public static bool IsLatitudeValid(double latitude)
        {
            if (latitude <= -200)
            {
                return false;
            }
            return true;
        }
    }
}
