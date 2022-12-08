namespace SacramentMeetingPlanner.Models
{
    public class SacramentMeeting
    {
        public int SacramentMeetingId { get; set; }
        public DateTime SacramentMeetingDate { get; set; }


        public ICollection<Event>? EventList { get; set; }
    }
}
