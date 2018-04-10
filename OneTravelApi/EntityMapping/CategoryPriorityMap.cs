using Microsoft.EntityFrameworkCore;
using OneTravelApi.DataLayer;
using OneTravelApi.EntityLayer;

namespace OneTravelApi.EntityMapping
{
    public class CategoryPriorityMap : IEntityMap
    {
        public void Map(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryPriority>(entity =>
            {
                entity.ToTable("CategoryPriorities");

                entity.Property(e => e.PriorityName)
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
