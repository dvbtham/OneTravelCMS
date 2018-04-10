using Microsoft.EntityFrameworkCore;
using OneTravelApi.DataLayer;
using OneTravelApi.EntityLayer;

namespace OneTravelApi.EntityMapping
{
    public class CategoryLocalTravelMap : IEntityMap
    {
        public void Map(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryLocalTravel>(entity =>
            {
                entity.ToTable("CategoryLocalTravels");

                entity.Property(e => e.LocalCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LocalName)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.Description)
                    .HasMaxLength(800);

                entity.Property(e => e.IsActive)
                    .IsRequired();

                entity.HasOne(x => x.City).WithMany(x => x.LocalTravels);

            });
        }
    }
}
