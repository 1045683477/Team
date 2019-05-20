﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Team.Infrastructure.DbContext;

namespace Team.Infrastructure.Migrations
{
    [DbContext(typeof(MyContext))]
    [Migration("20190512144427_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Team.Model.Model.Participants", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("Sex");

                    b.Property<int>("TeamId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.ToTable("Participantses");
                });

            modelBuilder.Entity("Team.Model.Model.Run", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<float>("Distance");

                    b.Property<string>("EndPlace")
                        .IsRequired();

                    b.Property<DateTime>("EndTime");

                    b.Property<float>("Kcal");

                    b.Property<float>("Speed");

                    b.Property<int>("SportFreeModel");

                    b.Property<string>("StarPlace")
                        .IsRequired();

                    b.Property<DateTime>("StarTime");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Runs");
                });

            modelBuilder.Entity("Team.Model.Model.RunParticipants", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<int>("RunTeamId");

                    b.Property<int>("Sex");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("RunTeamId");

                    b.ToTable("RunParticipantses");
                });

            modelBuilder.Entity("Team.Model.Model.RunTeam", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Count");

                    b.Property<DateTime>("CreationTime");

                    b.Property<string>("Introduction");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("RunTeams");
                });

            modelBuilder.Entity("Team.Model.Model.Statistical", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<float>("AllTime");

                    b.Property<float>("Distance");

                    b.Property<float>("Kcal");

                    b.Property<int>("SportFreeModel");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Statisticals");
                });

            modelBuilder.Entity("Team.Model.Model.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .IsRequired();

                    b.Property<DateTime>("AgreedTime");

                    b.Property<int>("AllCount");

                    b.Property<int>("Count");

                    b.Property<DateTime>("CreationTime");

                    b.Property<string>("Introduction");

                    b.Property<string>("Name");

                    b.Property<int>("Sport");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("Team.Model.Model.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Account")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<int>("Province");

                    b.Property<int>("Role");

                    b.Property<int>("RunTeamState");

                    b.Property<int>("Sex");

                    b.Property<int>("StudentId");

                    b.Property<int>("UniversityId");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Team.Model.Model.Participants", b =>
                {
                    b.HasOne("Team.Model.Model.Team", "Team")
                        .WithMany("Participantses")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Team.Model.Model.Run", b =>
                {
                    b.HasOne("Team.Model.Model.User", "User")
                        .WithMany("Runs")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Team.Model.Model.RunParticipants", b =>
                {
                    b.HasOne("Team.Model.Model.RunTeam", "RunTeam")
                        .WithMany("RunParticipantses")
                        .HasForeignKey("RunTeamId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Team.Model.Model.RunTeam", b =>
                {
                    b.HasOne("Team.Model.Model.User", "User")
                        .WithOne("RunTeam")
                        .HasForeignKey("Team.Model.Model.RunTeam", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Team.Model.Model.Statistical", b =>
                {
                    b.HasOne("Team.Model.Model.User", "User")
                        .WithMany("Statistical")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Team.Model.Model.Team", b =>
                {
                    b.HasOne("Team.Model.Model.User", "User")
                        .WithMany("Teams")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
