using Microsoft.EntityFrameworkCore;
using OneTravelApi.DataLayer;
using OneTravelApi.EntityLayer;

namespace OneTravelApi.EntityMapping
{
    public class PartnerMap : IEntityMap
    {
        public void Map(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Partner>(entity =>
            {
                entity.ToTable("Partners");

                entity.Property(e => e.TaxCode)
                    .HasMaxLength(50);

                entity.Property(e => e.PartnerName)
                    .HasMaxLength(450);

                entity.Property(e => e.Address)
                    .HasMaxLength(600);

                entity.Property(e => e.Telephone)
                    .HasMaxLength(50);

                entity.Property(e => e.Email)
                    .HasMaxLength(150);

                entity.Property(e => e.Website)
                    .HasMaxLength(100);

                entity.Property(e => e.Summary)
                    .HasMaxLength(2000);

                entity.Property(e => e.UpdateDate)
                    .IsRequired();

                // Relationships

                entity.HasOne(x => x.GroupPartner).WithMany(x => x.Partners);
                entity.HasOne(x => x.User).WithMany(x => x.Partners);

            });
        }
    }
}
