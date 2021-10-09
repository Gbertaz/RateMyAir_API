using System;
using Microsoft.EntityFrameworkCore;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AirQuality>(entity =>
            {
                entity.ToTable("AirQuality");

                entity.HasIndex(e => e.AirQualityId, "IX_AirQuality_AirQualityId")
                    .IsUnique();

                entity.Property(e => e.CreatedAt).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
