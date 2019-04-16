using Microsoft.EntityFrameworkCore;
using Team.Model.Model;

namespace Team.Infrastructure.DbContext
{
    public class MyContext:Microsoft.EntityFrameworkCore.DbContext
    {
        public MyContext(DbContextOptions<MyContext> options):base(options)
        {
            
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

            modelBuilder.Entity<Model.Model.Team>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Model.Model.Team>()
                .HasMany(x => x.Participantses)
                .WithOne(x => x.Team)
                .HasForeignKey(x => x.TeamId);

            modelBuilder.Entity<Model.Model.Team>()
                .Property(x => x.UserId).IsRequired();

            modelBuilder.Entity<Model.Model.Team>()
                .Property(x => x.AgreedTime).IsRequired();

            modelBuilder.Entity<Model.Model.Team>()
                .Property(x => x.CreationTime).IsRequired();

            modelBuilder.Entity<Model.Model.Team>()
                .Property(x => x.AllCount).IsRequired();

            modelBuilder.Entity<Model.Model.Team>()
                .Property(x => x.Count).IsRequired();

            modelBuilder.Entity<Model.Model.Team>()
                .Property(x => x.Note).IsRequired();

            modelBuilder.Entity<Model.Model.Team>()
                .Property(x => x.Sport).IsRequired();

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
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Model.Model.Team> Teams { get; set; }

        public DbSet<Participants> Participantses { get; set; }

        public DbSet<Run> Runs { get; set; }
    }
}
