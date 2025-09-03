using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CrickerManagmentSystem_API_.Models;

public partial class CricketLeagueContext : DbContext
{
    public CricketLeagueContext()
    {
    }

    public CricketLeagueContext(DbContextOptions<CricketLeagueContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Match> Matches { get; set; }

    public virtual DbSet<MatchScore> MatchScores { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<PointTable> PointTables { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<TeamWisePlayer> TeamWisePlayers { get; set; }

    public virtual DbSet<User> Users { get; set; }

  
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Match>(entity =>
        {
            entity.Property(e => e.MatchId).HasColumnName("MatchID");
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.StartTime).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TeamAid).HasColumnName("TeamAID");
            entity.Property(e => e.TeamBid).HasColumnName("TeamBID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Venue)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WinnerTeamId).HasColumnName("WinnerTeamID");

            entity.HasOne(d => d.TeamA).WithMany(p => p.MatchTeamAs)
                .HasForeignKey(d => d.TeamAid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Matches_Teams");

            entity.HasOne(d => d.TeamB).WithMany(p => p.MatchTeamBs)
                .HasForeignKey(d => d.TeamBid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Matches_Teams1");

            entity.HasOne(d => d.User).WithMany(p => p.Matches)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Matches_User");

            entity.HasOne(d => d.WinnerTeam).WithMany(p => p.MatchWinnerTeams)
                .HasForeignKey(d => d.WinnerTeamId)
                .HasConstraintName("FK_Matches_Teams2");
        });

        modelBuilder.Entity<MatchScore>(entity =>
        {
            entity.HasKey(e => e.ScoreId);

            entity.ToTable("MatchScore");

            entity.Property(e => e.ScoreId).HasColumnName("ScoreID");
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.MatchId).HasColumnName("MatchID");
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.TeamId).HasColumnName("TeamID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Match).WithMany(p => p.MatchScores)
                .HasForeignKey(d => d.MatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MatchScore_Matches");

            entity.HasOne(d => d.Team).WithMany(p => p.MatchScores)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MatchScore_Teams");

            entity.HasOne(d => d.User).WithMany(p => p.MatchScores)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MatchScore_User");
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.ToTable("Player");

            entity.Property(e => e.PlayerId).HasColumnName("PlayerID");
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Image)
                .HasMaxLength(500)
                .HasColumnName("image");
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.Nationality)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PlayerName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.TeamId).HasColumnName("TeamID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Team).WithMany(p => p.Players)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Player_Teams");

            entity.HasOne(d => d.User).WithMany(p => p.Players)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Player_User");
        });

        modelBuilder.Entity<PointTable>(entity =>
        {
            entity.ToTable("PointTable");

            entity.Property(e => e.PointTableId).HasColumnName("PointTableID");
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.TeamId).HasColumnName("TeamID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Team).WithMany(p => p.PointTables)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PointTable_Teams");

            entity.HasOne(d => d.User).WithMany(p => p.PointTables)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PointTable_User");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.Property(e => e.TeamId).HasColumnName("TeamID");
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.TeamDescription)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.TeamName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Teams)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Teams_User");
        });

        modelBuilder.Entity<TeamWisePlayer>(entity =>
        {
            entity.HasKey(e => e.TeamWisePlayerId).HasName("PK__TeamWise__49399691A3EC4875");

            entity.ToTable("TeamWisePlayer");

            entity.Property(e => e.TeamWisePlayerId).HasColumnName("TeamWisePlayerID");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Modified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PlayerId).HasColumnName("PlayerID");
            entity.Property(e => e.TeamId).HasColumnName("TeamID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Player).WithMany(p => p.TeamWisePlayers)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TeamWiseP__Playe__14270015");

            entity.HasOne(d => d.Team).WithMany(p => p.TeamWisePlayers)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TeamWiseP__TeamI__1332DBDC");

            entity.HasOne(d => d.User).WithMany(p => p.TeamWisePlayers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TeamWiseP__UserI__151B244E");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IsAdmin)
                .HasDefaultValue(false)
                .HasColumnName("isAdmin");
            entity.Property(e => e.Mobile)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
