using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SacramentMeetingPlanner.Models
{
    public class EventView
    {
        public int? EventId { get; set; }
        public int EventTypeId { get; set; }
        [DisplayName("Agenda Item")]
        public string? EventType { get; set; }
        [DisplayName("Agenda Item Description")]
        public string EventDescription { get; set; }
        [DisplayName("First Name")]
        public string? FirstName { get; set; }
        [DisplayName("Last Name")]
        public string? LastName { get; set; }
        public int? HymnId { get; set; }
        public string? Hymn { get; set; }
        public string? Topic { get; set; }
    }
}
