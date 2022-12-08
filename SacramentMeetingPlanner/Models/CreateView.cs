namespace SacramentMeetingPlanner.Models
{
    public class CreateView
    {
        public DateTime SacramentMeetingDate { get; set; }
        public List<Hymn> Hynns { get; set; }
        public List<EventType> EventTypes { get; set; }
    }
}
