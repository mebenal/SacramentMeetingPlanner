using Microsoft.AspNetCore.Mvc.Rendering;

namespace SacramentMeetingPlanner.Models
{
    public class CreateView
    {
        public DateTime SacramentMeetingDate { get; set; }
        public string Hynns { get; set; }
        public string EventTypes { get; set; }

        public ICollection<Event>? EventList { get; set; }
    }
}
