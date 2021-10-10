using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace RateMyAir.Entities.Models
{
    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AirQuality> AirQualities { get; set; }
        public virtual DbSet<IndexLevel> IndexLevels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AirQuality>(entity =>
            {
                entity.ToTable("AirQuality");

                entity.HasIndex(e => e.AirQualityId, "IX_AirQuality_AirQualityId")
                    .IsUnique();

                entity.Property(e => e.CreatedAt).IsRequired();
            });

            modelBuilder.Entity<IndexLevel>(entity =>
            {
                entity.HasIndex(e => e.IndexLevelId, "IX_IndexLevels_IndexLevelId")
                    .IsUnique();

                entity.Property(e => e.AirQualityIndex).IsRequired();

                entity.Property(e => e.Color).IsRequired();

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Pollutant).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
