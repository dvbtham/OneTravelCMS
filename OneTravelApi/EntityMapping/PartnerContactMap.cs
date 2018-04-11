using Microsoft.EntityFrameworkCore;
using OneTravelApi.DataLayer;
using OneTravelApi.EntityLayer;

namespace OneTravelApi.EntityMapping
{
    public class PartnerContactMap : IEntityMap
    {
        public void Map(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PartnerContact>(entity =>
            {
                entity.ToTable("PartnerContacts");

                entity.Property(e => e.ContactName)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.PositionTitle)
                    .HasMaxLength(150);

                entity.Property(e => e.Email)
                    .HasMaxLength(150);

                entity.Property(e => e.Mobile)
                    .HasMaxLength(80);

                entity.Property(e => e.Note)
                    .HasMaxLength(800);

                entity.Property(e => e.UpdateDate)
                    .IsRequired();

                entity.Property(e => e.IsActive)
                    .IsRequired();

                // Relationships

                entity.HasOne(x => x.Partner).WithMany(x => x.PartnerContacts).HasForeignKey(x => x.IdPartner);
                entity.HasOne(x => x.User).WithMany(x => x.PartnerContacts).HasForeignKey(x => x.UpdateByUser);

            });
        }
    }
}
