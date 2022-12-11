using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace SacramentMeetingPlanner.Models
{
    public class SacramentMeetingView
    {
        public int? SacramentMeetingId { get; set; }
        [DisplayName("Sacrament Meeting Date")]
        public DateTime SacramentMeetingDate { get; set; }
        public string? HymnsString { get; set; }
        public string? EventTypeString { get; set; }
        public List<Hymn>? Hymns { get; set; }
        public List<EventType>? EventType { get; set; }

        public List<EventView>? EventList { get; set; }
    }
}
