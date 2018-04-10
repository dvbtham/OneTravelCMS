using Microsoft.EntityFrameworkCore;
using OneTravelApi.DataLayer;
using OneTravelApi.EntityLayer;

namespace OneTravelApi.EntityMapping
{
    public class CategoryBookingStatusMap : IEntityMap
    {
        public void Map(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryBookingStatus>(entity =>
            {
                entity.ToTable("CategoryBookingStatuses");

                entity.Property(e => e.BookingStatusCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.BookingStatusName)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.Position)
                    .IsRequired();

                entity.Property(e => e.IsActive)
                    .IsRequired();

            });
        }
    }
}
