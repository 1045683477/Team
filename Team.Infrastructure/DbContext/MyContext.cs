using Microsoft.EntityFrameworkCore;
using Team.Model.Model.RunTeamModel;
using Team.Model.Model.TeamModel;
using Team.Model.Model.UserModel;

namespace Team.Infrastructure.DbContext
{
    public class MyContext:Microsoft.EntityFrameworkCore.DbContext
    {
        public MyContext(DbContextOptions<MyContext> options):base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region User

            modelBuilder.Entity<User>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<User>()
                .HasMany(x => x.Teams)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);

            modelBuilder.Entity<User>()
                .HasMany(x => x.Runs)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);

            modelBuilder.Entity<User>()
                .HasMany(x => x.Statistical)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);

            modelBuilder.Entity<User>()
                .Property(x => x.Account).IsRequired();
            modelBuilder.Entity<User>()
                .Property(x => x.Name).IsRequired();
            modelBuilder.Entity<User>()
                .Property(x => x.Password).IsRequired();
            modelBuilder.Entity<User>()
                .Property(x => x.Role).IsRequired();
            modelBuilder.Entity<User>()
                .Property(x => x.Sex).IsRequired();


            #endregion

            #region Team

            modelBuilder.Entity<Model.Model.TeamModel.Team>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Model.Model.TeamModel.Team>()
                .HasMany(x => x.Participantses)
                .WithOne(x => x.Team)
                .HasForeignKey(x => x.TeamId);

            modelBuilder.Entity<Model.Model.TeamModel.Team>()
                .Property(x => x.UserId).IsRequired();

            modelBuilder.Entity<Model.Model.TeamModel.Team>()
                .Property(x => x.AgreedTime).IsRequired();

            modelBuilder.Entity<Model.Model.TeamModel.Team>()
                .Property(x => x.CreationTime).IsRequired();

            modelBuilder.Entity<Model.Model.TeamModel.Team>()
                .Property(x => x.AllCount).IsRequired();

            modelBuilder.Entity<Model.Model.TeamModel.Team>()
                .Property(x => x.Count).IsRequired();

            modelBuilder.Entity<Model.Model.TeamModel.Team>()
                .Property(x => x.Address).IsRequired();

            modelBuilder.Entity<Model.Model.TeamModel.Team>()
                .Property(x => x.Sport).IsRequired();

            modelBuilder.Entity<Model.Model.TeamModel.Team>()
                .HasOne(x => x.User)
                .WithMany(x => x.Teams)
                .HasForeignKey(x => x.UserId);

            #endregion

            #region Run

            modelBuilder.Entity<Run>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Run>()
                .HasOne(x => x.User)
                .WithMany(x => x.Runs)
                .HasForeignKey(x => x.UserId);

            modelBuilder.Entity<Run>()
                .Property(x => x.UserId).IsRequired();

            modelBuilder.Entity<Run>()
                .Property(x => x.EndPlace).IsRequired();

            modelBuilder.Entity<Run>()
                .Property(x => x.EndTime).IsRequired();

            modelBuilder.Entity<Run>()
                .Property(x => x.StarTime).IsRequired();

            modelBuilder.Entity<Run>()
                .Property(x => x.StarPlace).IsRequired();

            modelBuilder.Entity<Run>()
                .Property(x => x.Distance).IsRequired();

            #endregion

            #region Participants

            modelBuilder.Entity<Participants>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Participants>()
                .HasOne(x => x.Team)
                .WithMany(x => x.Participantses)
                .HasForeignKey(x => x.TeamId);

            modelBuilder.Entity<Participants>()
                .Property(x => x.TeamId).IsRequired();

            modelBuilder.Entity<Participants>()
                .Property(x => x.UserId).IsRequired();

            modelBuilder.Entity<Participants>()
                .Property(x => x.Name).IsRequired();

            #endregion

            #region Statistical

            modelBuilder.Entity<Statistical>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Statistical>()
                .HasOne(x => x.User)
                .WithMany(x => x.Statistical)
                .HasForeignKey(x => x.UserId);

            #endregion

            #region RunTeam

            modelBuilder.Entity<RunTeam>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<RunTeam>()
                .Property(x => x.Name).IsRequired();
            modelBuilder.Entity<RunTeam>()
                .HasMany(x => x.RunParticipantses)
                .WithOne(x => x.RunTeam)
                .HasForeignKey(x=>x.RunTeamId);
            modelBuilder.Entity<RunTeam>()
                .HasOne(x => x.User)
                .WithOne(x => x.RunTeam)
                .HasForeignKey<RunTeam>(x => x.UserId);

            #endregion

            #region RunParticipants

            modelBuilder.Entity<RunParticipants>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<RunParticipants>()
                .HasOne(x => x.RunTeam)
                .WithMany(x => x.RunParticipantses)
                .HasForeignKey(x => x.RunTeamId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region RunTeamRecord

            modelBuilder.Entity<RunTeamRecord>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<RunTeamRecord>()
                .HasOne(x => x.RunTeam)
                .WithMany(x => x.RunTeamRecords)
                .HasForeignKey(x => x.RunTeamId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<RunTeamRecord>()
                .Property(x => x.DateTime)
                .HasColumnType("date");

            #endregion

            #region RunApplicants

            modelBuilder.Entity<RunApplicant>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<RunApplicant>()
                .HasOne(x => x.RunTeam)
                .WithMany(x => x.RunApplicants)
                .HasForeignKey(x => x.RunTeamId)
                .OnDelete(DeleteBehavior.Cascade);


            #endregion

            #region RunTimeDailyCharts

            modelBuilder.Entity<RunTimeDailyCharts>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<RunTimeDailyCharts>()
                .Property(x => x.DateTime).HasColumnType("date");

            #endregion

            #region RunTimeWeekCharts

            modelBuilder.Entity<RunTimeWeekCharts>()
                .HasKey(x => x.Id);

            #endregion
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Model.Model.TeamModel.Team> Teams { get; set; }

        public DbSet<Participants> Participantses { get; set; }

        public DbSet<Run> Runs { get; set; }

        public DbSet<Statistical> Statisticals { get; set; }

        #region RunTeam

        public DbSet<RunTeam> RunTeams { get; set; }

        public DbSet<RunParticipants> RunParticipantses { get; set; }

        public DbSet<RunTeamRecord> RunTeamRecords { get; set; }

        public DbSet<RunApplicant> RunApplicants { get; set; }

        public DbSet<RunTimeDailyCharts> RunTimeDailyChartses { get; set; }

        public DbSet<RunTimeWeekCharts> RunTimeWeekChartses { get; set; }

        public  DbSet<TeamList> TeamLists { get; set; }

        #endregion


        
    }
}
