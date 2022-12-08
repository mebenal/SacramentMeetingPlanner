using System.ComponentModel.DataAnnotations;
namespace SacramentMeetingPlanner.Models
{
    public class SacramentMeeting
    {
        public int Id { get; set; }
        [Display(Name = "Sacrament Date")]
        public DateTime SacramentDate { get; set; }
        public string Presiding { get; set; }
        public ICollection<Event>? EventList { get; set; }
    }
}
