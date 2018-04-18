using Microsoft.EntityFrameworkCore;
using OneTravelApi.DataLayer;
using OneTravelApi.EntityLayer;

namespace OneTravelApi.EntityMapping
{
    public class TravelRequestMap : IEntityMap
    {
        public void Map(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TravelRequest>(entity =>
            {
                entity.ToTable("TravelRequests");

                entity.Property(e => e.RequestCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.RequestName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.GuestMobile)
                    .HasMaxLength(80);

                entity.Property(e => e.GuestName)
                    .HasMaxLength(250);

                entity.Property(e => e.GuestEmail)
                    .HasMaxLength(250);

                entity.Property(e => e.IsActive)
                    .IsRequired();

                entity.HasOne(x => x.CategoryRequest).WithMany(x => x.TravelRequests)
                    .HasForeignKey(x => x.IdCategoryRequest);

                entity.HasOne(x => x.CategoryRequestStatus).WithMany(x => x.TravelRequests)
                    .HasForeignKey(x => x.IdCategoryRequestStatus);

                entity.HasOne(x => x.Partner).WithMany(x => x.TravelRequests)
                    .HasForeignKey(x => x.RequestByPartner);

                entity.HasOne(x => x.PartnerContact).WithMany(x => x.TravelRequests)
                    .HasForeignKey(x => x.RequestByPartnerContact);

                entity.HasOne(x => x.User).WithMany(x => x.TravelRequests)
                    .HasForeignKey(x => x.UpdateByUser);

            });
        }
    }
}
