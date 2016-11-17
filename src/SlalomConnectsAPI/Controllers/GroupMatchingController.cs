using SlalomConnectsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SlalomConnectsAPI.Controllers
{
    public class GroupMatchingController
    {
        private const int MinimumGroupSize = 2;
        private const int LunchBufferTime = 45;
        private const int CoffeeBufferTime = 20;
        private const int PingPongBufferTime = 15;
        private int _bufferTime;

        public Tuple<List<EventRequest>, List<EventRequest>> MatchGroupFromEventRequests(EventRequest newEventRequest,
            List<EventRequest> existingEventRequests)
        {
            if (existingEventRequests.Count < MinimumGroupSize) { return null; }

            _bufferTime = Int32.MaxValue;
            if (newEventRequest.EventType == EventType.Lunch)
            {
                _bufferTime = LunchBufferTime;
            }
            else if (newEventRequest.EventType == EventType.Coffee)
            {
                _bufferTime = CoffeeBufferTime;
            }
            else if (newEventRequest.EventType == EventType.PingPong)
            {
                _bufferTime = PingPongBufferTime;
            }

            if (newEventRequest.StartTime.AddMinutes(_bufferTime) > newEventRequest.EndTime){ return null; }

            var requestsWithMatchingEventType = GetRequestsWithMatchingEventType(newEventRequest, existingEventRequests);
            if (requestsWithMatchingEventType.Count < MinimumGroupSize) { return null; }

            return null;
        }

        private static List<EventRequest> GetRequestsWithMatchingEventType(EventRequest newEventRequest, List<EventRequest> existingEventRequests)
        {
            return existingEventRequests.Where(aRequest => newEventRequest.EventType == aRequest.EventType).ToList();
        }

        private static List<EventRequest> GetRequestsWithMatchingEventTimes(EventRequest newEventRequest,
            List<EventRequest> existingEventRequests)
        {
            var possibleStartTime = newEventRequest.StartTime;
            var possibleEndTime = newEventRequest.EndTime;

            foreach (var existingEventRequest in existingEventRequests)
            {
                //if(existingEventRequest.StartTime)
            }

            return null;
        }


    }
}