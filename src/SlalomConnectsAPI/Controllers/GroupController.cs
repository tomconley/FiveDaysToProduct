using SlalomConnectsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SlalomConnectsAPI.Controllers
{
    public class GroupController
    {
        private const int MinimumGroupSize = 2;
        private const int LunchBufferTime = 45;
        private const int CoffeeBufferTime = 20;
        private const int PingPongBufferTime = 15;
        private static int _bufferTime;

        private const int MinimumTimeInMinutesBeforeEvent = 10;

        public EventGroup MatchGroupFromEventRequests(EventRequest newEventRequest, List<EventRequest> existingEventRequests)
        {
            if (existingEventRequests.Count < MinimumGroupSize) { return null; }

            SetEventTypeBufferTime(newEventRequest);

            if (newEventRequest.StartTime.AddMinutes(_bufferTime) > newEventRequest.EndTime) { return null; }

            var requestsWithMatchingEventType = GetRequestsWithMatchingEventType(newEventRequest, existingEventRequests);
            if (requestsWithMatchingEventType.Count < MinimumGroupSize) { return null; }

            var possibleEventGroupsWithMinimumCountOrMore = GetEventGroupByMatchingEventTimes(newEventRequest, requestsWithMatchingEventType);

            if (possibleEventGroupsWithMinimumCountOrMore != null)
            {
                return GetEventGroupWithEarliestSubmitionTime(possibleEventGroupsWithMinimumCountOrMore);
            }

            return null;
        }

        private static void SetEventTypeBufferTime(EventRequest newEventRequest)
        {
            _bufferTime = int.MaxValue;

            switch (newEventRequest.EventType)
            {
                case EventType.Lunch:
                    _bufferTime = LunchBufferTime;
                    break;

                case EventType.Coffee:
                    _bufferTime = CoffeeBufferTime;
                    break;

                case EventType.PingPong:
                    _bufferTime = PingPongBufferTime;
                    break;
            }
        }

        public EventGroup CheckForGroupThatCouldAddTheNewEventRequest(EventRequest newEventRequest, List<EventGroup> existingEventGroups)
        {
            if (existingEventGroups.Count <= 0)
            {
                return null;
            }

            SetEventTypeBufferTime(newEventRequest);

            foreach (var existingEventGroup in existingEventGroups)
            {
                if (existingEventGroup.EventType != newEventRequest.EventType)
                {
                    continue;
                }

                if (existingEventGroup.StartTime.AddMinutes(_bufferTime) <= newEventRequest.EndTime
                    && newEventRequest.StartTime.AddMinutes(_bufferTime) <= existingEventGroup.EndTime)
                {
                    return existingEventGroup;
                }
            }

            return null;
        }

        public List<EventGroup> RemoveGroupsAtOrPastTheirStartTime(List<EventGroup> existingEventGroups)
        {
            return
                existingEventGroups.Where(
                    eventGroup => DateTime.Now.AddMinutes(MinimumTimeInMinutesBeforeEvent) < eventGroup.StartTime).ToList();
        }

        private static List<EventRequest> GetRequestsWithMatchingEventType(EventRequest newEventRequest, List<EventRequest> existingEventRequests)
        {
            return existingEventRequests.Where(aRequest => newEventRequest.EventType == aRequest.EventType).ToList();
        }

        private static List<EventGroup> GetEventGroupByMatchingEventTimes(EventRequest newEventRequest,
            List<EventRequest> existingEventRequests)
        {
            var possibleEventGroups = new List<EventGroup>();

            foreach (var existingEventRequest in existingEventRequests)
            {
                if (existingEventRequest.StartTime.AddMinutes(_bufferTime) > newEventRequest.EndTime) { continue; }
                if (newEventRequest.StartTime.AddMinutes(_bufferTime) > existingEventRequest.EndTime) { continue; }

                var possibleStartTime = newEventRequest.StartTime >= existingEventRequest.StartTime ? newEventRequest.StartTime : existingEventRequest.StartTime;
                var possibleEndTime = newEventRequest.EndTime <= existingEventRequest.EndTime ? newEventRequest.EndTime : existingEventRequest.EndTime;

                var newEventGroup = new EventGroup()
                {
                    EventRequests = new List<EventRequest>() { newEventRequest, existingEventRequest },
                    EventType = newEventRequest.EventType,
                    StartTime = possibleStartTime,
                    EndTime = possibleEndTime
                };

                possibleEventGroups.Add(newEventGroup);
            }

            foreach (var possibleEventGroup in possibleEventGroups)
            {
                foreach (var existingEventRequest in existingEventRequests)
                {
                    if (possibleEventGroup.EventRequests.Contains(existingEventRequest)) { continue; }
                    if (existingEventRequest.StartTime.AddMinutes(_bufferTime) > possibleEventGroup.EndTime) { continue; }
                    if (possibleEventGroup.StartTime.AddMinutes(_bufferTime) > existingEventRequest.EndTime) { continue; }

                    // If we made it here, the current request should fit the time range of the group. Check to see if the group is already 3 or more. If so, add this person without changing the start/end time.
                    if (possibleEventGroup.EventRequests.Count >= MinimumGroupSize)
                    {
                        possibleEventGroup.EventRequests.Add(existingEventRequest);
                        continue;
                    }

                    // Last case is if possibleEventGroup.EventRequests.Count == 2. Adjust the existing group time to fit the 3rd person, then add them to the group.
                    var possibleStartTime = possibleEventGroup.StartTime >= existingEventRequest.StartTime ? possibleEventGroup.StartTime : existingEventRequest.StartTime;
                    var possibleEndTime = possibleEventGroup.EndTime <= existingEventRequest.EndTime ? possibleEventGroup.EndTime : existingEventRequest.EndTime;

                    possibleEventGroup.EventRequests.Add(existingEventRequest);
                    possibleEventGroup.StartTime = possibleStartTime;
                    possibleEventGroup.EndTime = possibleEndTime;
                }
            }

            if (possibleEventGroups.Count == 0) return null;

            var possibleEventGroupsWithMinimumCountOrMore =
                possibleEventGroups.Where(aEventGroup => aEventGroup.EventRequests.Count > MinimumGroupSize).ToList();

            if (possibleEventGroupsWithMinimumCountOrMore.Count == 0) return null;

            return possibleEventGroupsWithMinimumCountOrMore;
        }

        private static EventGroup GetEventGroupWithEarliestSubmitionTime(List<EventGroup> eventGroups)
        {
            if (eventGroups.Count == 0) return null;

            var earliestSubmitionTime = DateTime.MaxValue;
            var eventGroupWithEarliestSubmitionTime = eventGroups[0];

            foreach (var eventGroup in eventGroups)
            {
                foreach (var eventRequest in eventGroup.EventRequests)
                {
                    if (eventRequest.TimeOfRequestSubmition < earliestSubmitionTime)
                    {
                        earliestSubmitionTime = eventRequest.TimeOfRequestSubmition;
                        eventGroupWithEarliestSubmitionTime = eventGroup;
                    }
                }
            }

            return eventGroupWithEarliestSubmitionTime;
        }
    }
}