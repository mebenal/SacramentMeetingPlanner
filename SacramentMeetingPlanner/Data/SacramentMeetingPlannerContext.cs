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

        public DbSet<SacramentMeeting> SacramentMeetings { get; set; } = default!;
        public DbSet<Event> Events {get; set;} = default!;
        public DbSet<EventType> EventTypes { get; set; } = default!;
        public DbSet<Hymn> Hymns { get; set; } = default!;
        public DbSet<Person> Person { get; set; } = default!;


        protected internal virtual void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<SacramentMeeting>().ToTable("sacrament_meeting");
            modelBuilder.Entity<Event>().ToTable("event");
            modelBuilder.Entity<EventType>().ToTable("event_type");
            modelBuilder.Entity<Hymn>().ToTable("hymn");
            modelBuilder.Entity<Person>().ToTable("person");
        }
    }
}
