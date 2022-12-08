namespace SacramentMeetingPlanner.Models
{
    public class SacramentMeetingView
    {
        public DateTime SacramentMeetingDate { get; set; }
        public ICollection<EventView>? EventList { get; set; }

    }
}
