using System.ComponentModel.DataAnnotations.Schema;

namespace SacramentMeetingPlanner.Models
{
    public class Event
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }
        public SacramentMeeting? Meeting { get; set; }
        public string localId { get; set;} 
        public string EventType { get; set; }
        public string EventDetails { get; set; }
    }
}
