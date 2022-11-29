namespace SacramentMeetingPlanner.Models
{
    public class SacramentMeeting
    {
        public int Id { get; set; }
        public DateTime SacramentDate { get; set; }
        public List<Event> EventList { get; set; }
    }
}
