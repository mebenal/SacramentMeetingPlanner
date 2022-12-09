using Microsoft.AspNetCore.Mvc.Rendering;

namespace SacramentMeetingPlanner.Models
{
    public class SacramentMeetingView
    {
        public int? SacramentMeetingId { get; set; }
        public DateTime SacramentMeetingDate { get; set; }
        public string Hymns { get; set; }
        public string EventTypes { get; set; }

        public ICollection<EventView>? EventList { get; set; }
    }
}
