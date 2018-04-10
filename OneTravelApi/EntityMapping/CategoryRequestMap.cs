using Microsoft.EntityFrameworkCore;
using OneTravelApi.DataLayer;
using OneTravelApi.EntityLayer;

namespace OneTravelApi.EntityMapping
{
    public class CategoryRequestMap : IEntityMap
    {
        public void Map(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryRequest>(entity =>
            {
                entity.ToTable("CategoryRequests");

                entity.Property(e => e.RequestName)
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
