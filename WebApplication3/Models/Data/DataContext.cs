using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace MLB_App.Models.Data
{
    public class DataContext : DbContext
    {
        public DataContext() : base("DataContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            this.Configuration.AutoDetectChangesEnabled = true;

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<Schedule>()
            .HasRequired(s => s.probableStartingPitchers)
            .WithMany(p => p.Schedules)
            .HasForeignKey(s => s.ProbableStartingPitchers_Id);
            modelBuilder.Entity<Player>().HasKey(p => p.playerID);
            modelBuilder.Entity<ProbableStartingPitchers>().HasKey(p => p.Id);
            modelBuilder.Entity<RealTimeBoxScore>()
                .HasRequired(l => l.lineScore)
                .WithMany(r => r.RealTimeBoxScores)
                .HasForeignKey(l => l.lineScore_Id);
            modelBuilder.Entity<lineScore>()
                .HasRequired(x => x.away)
                .WithMany(x => x.lineScores)
                .HasForeignKey(x => x.away_Id);
            modelBuilder.Entity<lineScore>()
                .HasRequired(x => x.home)
                .WithMany(x => x.lineScores)
                .HasForeignKey(x => x.home_Id);
            modelBuilder.Entity<GameDetails>()
                .HasOptional(l => l.lineScore)
                .WithMany(g => g.GameDetails)
                .HasForeignKey(l => l.lineScore_Id);
        }

        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<ProbableStartingPitchers> ProbableStartingPitchers { get; set; }
        public DbSet<RealTimeBoxScore> RealTimeBoxScore { get; set; }
        public DbSet<GameDetails> GameDetails { get; set; }
    }
}