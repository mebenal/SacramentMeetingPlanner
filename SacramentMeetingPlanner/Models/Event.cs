using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;

namespace SacramentMeetingPlanner.Models
{
    public class Event
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int EventId { get; set; }
        [Required]
        public int EventTypeId { get; set; }
        [Required]
        public int SacramentMeetingId { get; set; }
        public int? PrevEventId { get; set; }
        [Required]
        public int RowId { get; set; }
        [Required]
        public string EventDescription { get; set; }
        public string? Topic { get; set; }
        
        public EventType EventType { get; set; }
        [ForeignKey("SacramentMeetingId")]
        public SacramentMeeting SacramentMeeting { get; set; }
        [ForeignKey("PrevEventId")]
        public Event? PrevEvent { get; set; }
    }
}
