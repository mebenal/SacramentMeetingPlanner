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
    [DbContext(typeof(SacramentDbContext))]
    [Migration("20221129161741_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.10");

            modelBuilder.Entity("SacramentMeetingPlanner.Models.Event", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("EventType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("SacramentMeetingId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("SacramentMeetingId");

                    b.ToTable("Event");
                });

            modelBuilder.Entity("SacramentMeetingPlanner.Models.SacramentMeeting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("SacramentDate")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("SacramentMeeting");
                });

            modelBuilder.Entity("SacramentMeetingPlanner.Models.Event", b =>
                {
                    b.HasOne("SacramentMeetingPlanner.Models.SacramentMeeting", null)
                        .WithMany("EventList")
                        .HasForeignKey("SacramentMeetingId");
                });

            modelBuilder.Entity("SacramentMeetingPlanner.Models.SacramentMeeting", b =>
                {
                    b.Navigation("EventList");
                });
#pragma warning restore 612, 618
        }
    }
}
