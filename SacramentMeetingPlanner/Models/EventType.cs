namespace SacramentMeetingPlanner.Models
{
    public class EventType
    {
        public int EventTypeId { get; set; }
        public string EventTypeName { get; set; }

        ICollection<Event> Events { get; set; }
    }
}
