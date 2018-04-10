using Microsoft.EntityFrameworkCore;
using OneTravelApi.DataLayer;
using OneTravelApi.EntityLayer;

namespace OneTravelApi.EntityMapping
{
    public class CategoryRequestStatusMap : IEntityMap
    {
        public void Map(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryRequestStatus>(entity =>
            {
                entity.ToTable("CategoryRequestStatuses");

                entity.Property(e => e.RequestStatusCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.RequestStatusName)
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
