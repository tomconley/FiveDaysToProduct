using System;

namespace SlalomConnectsAPI.Models
{
    public class EventRequest
    {
        public Guid EventRequestGuid { get; set; }
        public DateTime TimeOfRequestSubmition { get; set; }
        public string Email { get; set; }
        public EventType EventType { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int? MaximumGroupSize { get; set; }

        public override string ToString()
        {
            return "Email: " + Email + " Event Type: " + EventType + " StartTime: " + StartTime + " EndTime: " + EndTime + " Max Group Size: " + MaximumGroupSize;
        }
    }
}