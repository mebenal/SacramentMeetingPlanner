using System.ComponentModel.DataAnnotations.Schema;

namespace SacramentMeetingPlanner.Models
{
    public class Event
    {
        public string Id { get; set; }
        public string EventType { get; set; }
        
        [NotMapped]
        public Dictionary<string, object> keyValuePairs { get; set; }
    }
}
