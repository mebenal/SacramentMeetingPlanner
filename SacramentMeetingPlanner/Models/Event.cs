using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;

namespace SacramentMeetingPlanner.Models
{
    public class Event
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EventId { get; set; }
        public int EventTypeId { get; set; }
        public int SacramentMeetingId { get; set; }
        public int? NextEventId { get; set; }
        public int RowId { get; set; }
        public string EventDescription { get; set; }
        
        public EventType EventType { get; set; }
        public SacramentMeeting SacramentMeeting { get; set; }
        public Event? NextEvent { get; set; }
    }
}
