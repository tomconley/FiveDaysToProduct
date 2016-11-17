using System;
using System.Collections.Generic;

namespace SlalomConnectsAPI.Models
{
    public class EventGroup
    {
        public List<EventRequest> EventRequests { get; set; }
        public EventType EventType { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}