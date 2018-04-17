using Microsoft.EntityFrameworkCore;
using OneTravelApi.DataLayer;
using OneTravelApi.EntityLayer;

namespace OneTravelApi.EntityMapping
{
    public class TravelPriceMap : IEntityMap
    {
        public void Map(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TravelPrice>(entity =>
            {
                entity.ToTable("TravelPrices");

                entity.Property(e => e.PriceName)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.Price)
                    .IsRequired();

                entity.Property(e => e.ContactInfo)
                    .HasMaxLength(2000);

                entity.Property(e => e.UpdateDate)
                    .IsRequired();

                entity.Property(e => e.IsActive)
                    .IsRequired();
                
                // Relationships

                entity.HasOne(x => x.User).WithMany(x => x.TravelPrices).HasForeignKey(x => x.UpdateByUser);
                entity.HasOne(x => x.CategoryPrice).WithMany(x => x.TravelPrices).HasForeignKey(x => x.IdCategoryPrice);
                entity.HasOne(x => x.CategoryLocalTravel)
                       .WithMany(x => x.TravelPrices)
                        .HasForeignKey(x => x.IdCategoryLocalTravel);
            });
        }
    }
}
