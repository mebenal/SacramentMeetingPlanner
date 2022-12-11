namespace SacramentMeetingPlanner.Models
{
    public class CalendarModel
    {
        public IEnumerable<SacramentMeeting> Meetings { get; set; }
        public Calendar Calendar { get; set; }
    }
}