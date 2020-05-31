using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FlightControlWeb.Controllers;
using FlightControlWeb.DataBaseClasses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ControllersTests
{
    [TestClass]
    public class FlightPlanControllerTests
    {
        [TestMethod]
        public async Task TestGetFlightPlan()
        {
            //Arrange
            // the plan to post
            FlightPlan plan = new FlightPlan(1, "1", new Location(0, 0, DateTime.UtcNow), new LinkedList<Segment>());
            var planID = "as1234";


            var plansDictStub = new ConcurrentDictionary<string, FlightPlan>();
            plansDictStub.TryAdd(planID, plan);

            var httpFactoryMock = new Mock<IHttpClientFactory>();
            httpFactoryMock.Setup(f => f.CreateClient("api")).Returns(new HttpClient());


            //the controller
            FlightPlanController controller = new FlightPlanController(
                plansDictStub,
                new Dictionary<string, Server>(),
                new Dictionary<string, string>(),
                httpFactoryMock.Object
            );
            var expected = plan;
            //Act
            var respone = await controller.GetFlightPlan(planID);
            var actual = respone.Result as OkObjectResult;

            //Assert
            if (actual != null) Assert.AreEqual(expected, actual.Value);
        }
    }
}
