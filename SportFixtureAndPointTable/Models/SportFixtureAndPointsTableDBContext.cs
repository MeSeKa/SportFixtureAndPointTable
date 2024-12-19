using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SportFixtureAndPointTable.Models;

public partial class SportFixtureAndPointsTableDBContext : DbContext
{
    public SportFixtureAndPointsTableDBContext()
    {
    }

    public SportFixtureAndPointsTableDBContext(DbContextOptions<SportFixtureAndPointsTableDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Fixture> Fixtures { get; set; }

    public virtual DbSet<MatchResult> MatchResults { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\MehmedSefa\\OneDrive\\Belgeler\\SportFixtureAndPointsTableDB.mdf;Integrated Security=True;Connect Timeout=30")
            .LogTo(Console.WriteLine); // SQL sorgularını konsola yazdır
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Fixture tablosu
        modelBuilder.Entity<Fixture>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Fixture__3214EC07E19F63A4");

            entity.ToTable("Fixture");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd(); // ID otomatik artış
            entity.Property(e => e.AwayTeamId).HasColumnName("awayTeamID");
            entity.Property(e => e.HomeTeamId).HasColumnName("homeTeamID");
            entity.Property(e => e.MatchDate).HasColumnName("matchDate");

            entity.HasOne(d => d.AwayTeam).WithMany(p => p.FixtureAwayTeams)
                .HasForeignKey(d => d.AwayTeamId)
                .HasConstraintName("FK_Fixture_AwayTeam");

            entity.HasOne(d => d.HomeTeam).WithMany(p => p.FixtureHomeTeams)
                .HasForeignKey(d => d.HomeTeamId)
                .HasConstraintName("FK_Fixture_HomeTeam");
        });

        // MatchResult tablosu
        modelBuilder.Entity<MatchResult>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MatchRes__3214EC0716912B15");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd(); // ID otomatik artış
            entity.Property(e => e.AwayScore).HasColumnName("awayScore");
            entity.Property(e => e.FixtureId).HasColumnName("fixtureId");
            entity.Property(e => e.HomeScore).HasColumnName("homeScore");
        });

        // Player tablosu
        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Player__3214EC07688F39D9");

            entity.ToTable("Player");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd(); // ID otomatik artış
            entity.Property(e => e.PlayerName)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("playerName");
            entity.Property(e => e.Position)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("position");
            entity.Property(e => e.TeamId).HasColumnName("teamId");

            entity.HasOne(d => d.Team).WithMany(p => p.Players)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("FK_Player_Teams");
        });

        // Team tablosu
        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.TeamId).HasName("PK__Teams__5ED7534ACA0D6B03");

            entity.Property(e => e.TeamId)
                .ValueGeneratedOnAdd() // ID otomatik artış
                .HasColumnName("teamID");
            entity.Property(e => e.City)
                .HasMaxLength(25)
                .IsFixedLength()
                .HasColumnName("city");
            entity.Property(e => e.TeamName)
                .HasMaxLength(30)
                .IsFixedLength()
                .HasColumnName("teamName");
        });

        OnModelCreatingPartial(modelBuilder);
    }


    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
