// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SacramentMeetingPlanner.Data;

#nullable disable

namespace SacramentMeetingPlanner.Migrations
{
    [DbContext(typeof(SacramentMeetingPlannerContext))]
    [Migration("20221209075224_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("SacramentMeetingPlanner.Models.Event", b =>
                {
                    b.Property<int>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EventId"), 1L, 1);

                    b.Property<string>("EventDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EventTypeId")
                        .HasColumnType("int");

                    b.Property<int?>("PrevEventId")
                        .HasColumnType("int");

                    b.Property<int>("RowId")
                        .HasColumnType("int");

                    b.Property<int>("SacramentMeetingId")
                        .HasColumnType("int");

                    b.Property<string>("Topic")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EventId");

                    b.HasIndex("EventTypeId");

                    b.HasIndex("PrevEventId");

                    b.HasIndex("SacramentMeetingId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("SacramentMeetingPlanner.Models.EventType", b =>
                {
                    b.Property<int>("EventTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EventTypeId"), 1L, 1);

                    b.Property<string>("EventTypeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EventTypeId");

                    b.ToTable("EventTypes");
                });

            modelBuilder.Entity("SacramentMeetingPlanner.Models.Hymn", b =>
                {
                    b.Property<int>("HymnId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("HymnId"), 1L, 1);

                    b.Property<string>("HymnName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("HymnNumber")
                        .HasColumnType("int");

                    b.HasKey("HymnId");

                    b.ToTable("Hymns");
                });

            modelBuilder.Entity("SacramentMeetingPlanner.Models.Person", b =>
                {
                    b.Property<int>("PersonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PersonId"), 1L, 1);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PersonId");

                    b.ToTable("Person");
                });

            modelBuilder.Entity("SacramentMeetingPlanner.Models.SacramentMeeting", b =>
                {
                    b.Property<int>("SacramentMeetingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SacramentMeetingId"), 1L, 1);

                    b.Property<DateTime>("SacramentMeetingDate")
                        .HasColumnType("datetime2");

                    b.HasKey("SacramentMeetingId");

                    b.ToTable("SacramentMeetings");
                });

            modelBuilder.Entity("SacramentMeetingPlanner.Models.Event", b =>
                {
                    b.HasOne("SacramentMeetingPlanner.Models.EventType", "EventType")
                        .WithMany()
                        .HasForeignKey("EventTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SacramentMeetingPlanner.Models.Event", "PrevEvent")
                        .WithMany()
                        .HasForeignKey("PrevEventId");

                    b.HasOne("SacramentMeetingPlanner.Models.SacramentMeeting", "SacramentMeeting")
                        .WithMany("EventList")
                        .HasForeignKey("SacramentMeetingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EventType");

                    b.Navigation("PrevEvent");

                    b.Navigation("SacramentMeeting");
                });

            modelBuilder.Entity("SacramentMeetingPlanner.Models.SacramentMeeting", b =>
                {
                    b.Navigation("EventList");
                });
#pragma warning restore 612, 618
        }
    }
}
