using Microsoft.AspNetCore.Mvc.Rendering;

namespace SacramentMeetingPlanner.Models
{
    public class CreateView
    {
        public DateTime SacramentMeetingDate { get; set; }
        public SelectList Hymns { get; set; }
        public SelectList EventTypes { get; set; }
    }
}
