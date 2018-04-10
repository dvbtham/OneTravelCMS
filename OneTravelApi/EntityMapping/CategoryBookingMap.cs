using Microsoft.EntityFrameworkCore;
using OneTravelApi.DataLayer;
using OneTravelApi.EntityLayer;

namespace OneTravelApi.EntityMapping
{
    public class CategoryBookingMap : IEntityMap
    {
        public void Map(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryBooking>(entity =>
            {
                entity.ToTable("CategoryBookings");

                entity.Property(e => e.BookingName)
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
