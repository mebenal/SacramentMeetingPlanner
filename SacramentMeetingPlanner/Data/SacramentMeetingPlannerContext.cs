using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SacramentMeetingPlanner.Models;

namespace SacramentMeetingPlanner.Data
{
    public class SacramentMeetingPlannerContext : DbContext
    {
        public SacramentMeetingPlannerContext (DbContextOptions<SacramentMeetingPlannerContext> options)
            : base(options)
        {
        }
        public DbSet<SacramentMeeting> SacramentMeeting { get; set; } = default!;
        public DbSet<Event> Event {get;set;} = default!;

        protected internal virtual void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<SacramentMeeting>()
                .HasMany(c => c.EventList)
                .WithOne(e => e.Meeting);
        }
    }
}
