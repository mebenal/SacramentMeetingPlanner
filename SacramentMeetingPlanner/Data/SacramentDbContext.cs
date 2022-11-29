using Microsoft.EntityFrameworkCore;
using SacramentMeetingPlanner.Models;

namespace SacramentMeetingPlanner.Data
{
    public class SacramentDbContext : DbContext
    {
        public SacramentDbContext(DbContextOptions<SacramentDbContext> options)
            : base(options)
        {
        }



        public DbSet<SacramentMeeting> SacramentMeeting { get; set; } = default!;
        public DbSet<Event> Event { get; set; } = default!;
    }
}
