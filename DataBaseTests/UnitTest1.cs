using System;
using System.Collections.Generic;
using FlightControlWeb;
using FlightControlWeb.DataBaseClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataBaseTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestInterpolation()
        {
            double expected = 240;
            /*Location location = new Location(34.0,34.0,DateTime.Now);
            FlightPlan plan = new FlightPlan("W1234IL",100,"wizzair",location,new LinkedList<Segment>());*/
            double start = 100;
            double end = 400;
            DateTime startTime = new DateTime(2020,05,20,7,40,0);
            DateTime currentTime = startTime.AddSeconds(70);
            int timespan = 150;
            var actual = Utiles.LinearInterpolation(start, end, startTime, timespan, currentTime);
            Assert.AreEqual(expected,actual);
        }
        [TestMethod]
        public void TestGetSigment()
        {
            FlightPlan plan;
            Location location = new Location(34.0, 32.2, DateTime.Now);
            double lat = 34.2, lon = 32.4;
            int time = 500;
            LinkedList<Segment> segments = new LinkedList<Segment>();
            for (var i = 0; i < 10; i++)
            {
                Segment seg = new Segment(lat,lon,time);
                lat += 0.2;
                lon += 0.2;
                segments.AddLast(seg);
            }

            plan = new FlightPlan(100, "wizzair", location, segments);

            int actual;
            int expected = 2;
            actual = plan.FindSegment(DateTime.Now.AddSeconds(1200));
            Assert.AreEqual(actual,expected);

        }

        
    }
}
