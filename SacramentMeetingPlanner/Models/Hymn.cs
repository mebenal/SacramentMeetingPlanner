using System.ComponentModel.DataAnnotations.Schema;

namespace SacramentMeetingPlanner.Models
{
    public class Hymn
    {
        public int HymnId { get; set; }
        public int HymnNumber { get; set; }
        public string HymnName { get; set; }

        [NotMapped]
        public string FullHymn
        {
            get { return $"{HymnNumber} {HymnName}"; }
        }
    }
}
