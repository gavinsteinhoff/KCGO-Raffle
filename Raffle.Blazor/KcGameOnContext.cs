using Microsoft.EntityFrameworkCore;

namespace Raffle.Blazor;

internal partial class KcGameOnContext(DbContextOptions<KcGameOnContext> options) : DbContext(options)
{
    internal virtual DbSet<Waiver> Waivers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Waiver>(entity =>
        {
            entity.HasKey(e => e.TableId).HasName("PRIMARY");

            entity.ToTable("Waiver", tb => tb.HasComment("to track who and when the waivers were signed.  This should be done each year."));

            entity.Property(e => e.TableId).HasColumnName("tableID");
            entity.Property(e => e.Active)
                .HasDefaultValueSql("'1'")
                .HasComment("0 = nonactive, player dropped from tournament.")
                .HasColumnName("active");
            entity.Property(e => e.Datetime)
                .HasColumnType("datetime")
                .HasColumnName("datetime");
            entity.Property(e => e.EventId).HasColumnName("eventId");
            entity.Property(e => e.IsMinor)
                .HasDefaultValueSql("'0'")
                .HasColumnName("isMinor");
            entity.Property(e => e.SignedName)
                .HasMaxLength(100)
                .HasColumnName("signedName");
            entity.Property(e => e.Tt)
                .HasMaxLength(10)
                .HasColumnName("TT");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");
            entity.Property(e => e.Wondoor)
                .HasMaxLength(50)
                .HasDefaultValueSql("'0'")
                .HasComment("0 = available to win, 1= won, 2= epic fail")
                .HasColumnName("wondoor");
            entity.Property(e => e.Wonloyalty)
                .HasMaxLength(50)
                .HasDefaultValueSql("'0'")
                .HasColumnName("wonloyalty");
            entity.Property(e => e.WonDateTime)
                .HasColumnType("datetime")
                .HasColumnName("wonDateTime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
