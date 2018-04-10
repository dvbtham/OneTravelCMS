using Microsoft.EntityFrameworkCore;
using OneTravelApi.DataLayer;
using OneTravelApi.EntityLayer;

namespace OneTravelApi.EntityMapping
{
    public class CategoryGroupPartnerMap : IEntityMap
    {
        public void Map(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryGroupPartner>(entity =>
            {
                entity.ToTable("CategoryGroupPartners");

                entity.Property(e => e.GroupPartnerCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.GroupPartnerName)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.Position)
                    .IsRequired();

                entity.Property(e => e.IsLocal)
                    .IsRequired();

                entity.Property(e => e.IsActive)
                    .IsRequired();

            });
        }
    }
}
