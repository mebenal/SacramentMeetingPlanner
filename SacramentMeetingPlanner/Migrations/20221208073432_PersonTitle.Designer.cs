﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SacramentMeetingPlanner.Data;

#nullable disable

namespace SacramentMeetingPlanner.Migrations
{
    [DbContext(typeof(SacramentMeetingPlannerContext))]
    [Migration("20221208073432_PersonTitle")]
    partial class PersonTitle
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.10");

            modelBuilder.Entity("SacramentMeetingPlanner.Models.Event", b =>
                {
                    b.Property<int>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("EventDescription")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("EventTypeId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("NextEventId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RowId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SacramentMeetingId")
                        .HasColumnType("INTEGER");

                    b.HasKey("EventId");

                    b.HasIndex("EventTypeId");

                    b.HasIndex("NextEventId");

                    b.HasIndex("SacramentMeetingId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("SacramentMeetingPlanner.Models.EventType", b =>
                {
                    b.Property<int>("EventTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("EventTypeName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("EventTypeId");

                    b.ToTable("EventTypes");
                });

            modelBuilder.Entity("SacramentMeetingPlanner.Models.Hymn", b =>
                {
                    b.Property<int>("HymnId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("HymnName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("HymnNumber")
                        .HasColumnType("INTEGER");

                    b.HasKey("HymnId");

                    b.ToTable("Hymns");
                });

            modelBuilder.Entity("SacramentMeetingPlanner.Models.Person", b =>
                {
                    b.Property<int>("PersonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("PersonId");

                    b.ToTable("Person");
                });

            modelBuilder.Entity("SacramentMeetingPlanner.Models.SacramentMeeting", b =>
                {
                    b.Property<int>("SacramentMeetingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("SacramentMeetingDate")
                        .HasColumnType("TEXT");

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

                    b.HasOne("SacramentMeetingPlanner.Models.Event", "NextEvent")
                        .WithMany()
                        .HasForeignKey("NextEventId");

                    b.HasOne("SacramentMeetingPlanner.Models.SacramentMeeting", "SacramentMeeting")
                        .WithMany("EventList")
                        .HasForeignKey("SacramentMeetingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EventType");

                    b.Navigation("NextEvent");

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
