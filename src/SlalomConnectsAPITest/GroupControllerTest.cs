using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlalomConnectsAPI.Controllers;
using SlalomConnectsAPI.Models;
using System;
using System.Collections.Generic;

namespace SlalomConnectsAPITest
{
    /// <summary>
    /// Summary description for GroupMatchingControllerTest
    /// </summary>
    [TestClass]
    public class GroupControllerTest
    {
        private readonly GroupController _groupMatchingController;

        public GroupControllerTest()
        {
            _groupMatchingController = new GroupController();
        }

        #region Additional test attributes

        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //

        #endregion Additional test attributes

        [TestMethod]
        public void
            WhenTwoRequestsForLunchExist_AndAThirdRequestForLunchIsCreated_GiveTheTimesAllMatch_AGroupShouldBeFormed_()
        {
            var existingEventRequests = new List<EventRequest>
            {
                new EventRequest()
                {
                    EventRequestGuid = Guid.NewGuid(),
                    TimeOfRequestSubmition = DateTime.Now,
                    Email = "cody@slalom.com",
                    EventType = EventType.Lunch,
                    StartTime = DateTime.Now.AddHours(1),
                    EndTime = DateTime.Now.AddHours(5)
                },
                new EventRequest()
                {
                    EventRequestGuid = Guid.NewGuid(),
                    TimeOfRequestSubmition = DateTime.Now,
                    Email = "tom@slalom.com",
                    EventType = EventType.Lunch,
                    StartTime = DateTime.Now.AddHours(1),
                    EndTime = DateTime.Now.AddHours(2)
                }
            };

            var newEventRequest = new EventRequest()
            {
                EventRequestGuid = Guid.NewGuid(),
                TimeOfRequestSubmition = DateTime.Now,
                Email = "josh@slalom.com",
                EventType = EventType.Lunch,
                StartTime = DateTime.Now.AddHours(1),
                EndTime = DateTime.Now.AddHours(3)
            };

            var groupResult = _groupMatchingController.MatchGroupFromEventRequests(newEventRequest, existingEventRequests);

            Assert.IsNotNull(groupResult);
        }

        [TestMethod]
        public void WhenTwoRequestsForLunchExist_AndAThirdRequestForLunchIsCreated_ButTheTimesDoNotMatch_AGroupShouldNotBeFormed_()
        {
            var existingEventRequests = new List<EventRequest>
            {
                new EventRequest()
                {
                    EventRequestGuid = Guid.NewGuid(),
                    TimeOfRequestSubmition = DateTime.Now,
                    Email = "cody@slalom.com",
                    EventType = EventType.Lunch,
                    StartTime = DateTime.Now.AddHours(1),
                    EndTime = DateTime.Now.AddHours(5)
                },
                new EventRequest()
                {
                    EventRequestGuid = Guid.NewGuid(),
                    TimeOfRequestSubmition = DateTime.Now,
                    Email = "tom@slalom.com",
                    EventType = EventType.Lunch,
                    StartTime = DateTime.Now.AddHours(1),
                    EndTime = DateTime.Now.AddHours(2)
                }
            };

            var newEventRequest = new EventRequest()
            {
                EventRequestGuid = Guid.NewGuid(),
                TimeOfRequestSubmition = DateTime.Now,
                Email = "josh@slalom.com",
                EventType = EventType.Lunch,
                StartTime = DateTime.Now.AddHours(6),
                EndTime = DateTime.Now.AddHours(10)
            };

            var groupResult = _groupMatchingController.MatchGroupFromEventRequests(newEventRequest, existingEventRequests);

            Assert.IsNull(groupResult);
        }

        [TestMethod]
        public void WhenTwoRequestsForLunchExist_AndAThirdRequestButForCoffeeIsCreated_AGroupShouldNotBeFormed()
        {
            var existingEventRequests = new List<EventRequest>
            {
                new EventRequest()
                {
                    EventRequestGuid = Guid.NewGuid(),
                    TimeOfRequestSubmition = DateTime.Now,
                    Email = "cody@slalom.com",
                    EventType = EventType.Lunch,
                    StartTime = DateTime.Now.AddHours(1),
                    EndTime = DateTime.Now.AddHours(5)
                },
                new EventRequest()
                {
                    EventRequestGuid = Guid.NewGuid(),
                    TimeOfRequestSubmition = DateTime.Now,
                    Email = "tom@slalom.com",
                    EventType = EventType.Lunch,
                    StartTime = DateTime.Now.AddHours(1),
                    EndTime = DateTime.Now.AddHours(2)
                }
            };

            var newEventRequest = new EventRequest()
            {
                EventRequestGuid = Guid.NewGuid(),
                TimeOfRequestSubmition = DateTime.Now,
                Email = "josh@slalom.com",
                EventType = EventType.Coffee,
                StartTime = DateTime.Now.AddHours(1),
                EndTime = DateTime.Now.AddHours(3)
            };

            var groupResult = _groupMatchingController.MatchGroupFromEventRequests(newEventRequest, existingEventRequests);

            Assert.IsNull(groupResult);
        }

        [TestMethod]
        public void WhenALunchGroupExists_AndFourthLunchRequestIsCreated_GivenTheTimesMatch_ShouldBeAbleToJoin()
        {
            var existingEventRequests = new List<EventRequest>
            {
                new EventRequest()
                {
                    EventRequestGuid = Guid.NewGuid(),
                    TimeOfRequestSubmition = DateTime.Now,
                    Email = "cody@slalom.com",
                    EventType = EventType.Lunch,
                    StartTime = DateTime.Now.AddHours(1),
                    EndTime = DateTime.Now.AddHours(5)
                },
                new EventRequest()
                {
                    EventRequestGuid = Guid.NewGuid(),
                    TimeOfRequestSubmition = DateTime.Now,
                    Email = "tom@slalom.com",
                    EventType = EventType.Lunch,
                    StartTime = DateTime.Now.AddHours(1),
                    EndTime = DateTime.Now.AddHours(2)
                }
            };

            var newEventRequest = new EventRequest()
            {
                EventRequestGuid = Guid.NewGuid(),
                TimeOfRequestSubmition = DateTime.Now,
                Email = "josh@slalom.com",
                EventType = EventType.Lunch,
                StartTime = DateTime.Now.AddHours(1),
                EndTime = DateTime.Now.AddHours(3)
            };

            var groupResult = _groupMatchingController.MatchGroupFromEventRequests(newEventRequest,
                existingEventRequests);

            var existingGroups = new List<EventGroup>()
            {
                groupResult
            };

            var secondNewEventRequest = new EventRequest()
            {
                EventRequestGuid = Guid.NewGuid(),
                TimeOfRequestSubmition = DateTime.Now,
                Email = "tim@slalom.com",
                EventType = EventType.Lunch,
                StartTime = DateTime.Now.AddHours(1),
                EndTime = DateTime.Now.AddHours(3)
            };

            var existingGroupResult =
                _groupMatchingController.CheckForGroupThatCouldAddTheNewEventRequest(secondNewEventRequest,
                    existingGroups);

            Assert.IsNotNull(existingGroupResult);
        }
    }
}