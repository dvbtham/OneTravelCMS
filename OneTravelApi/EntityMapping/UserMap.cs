using Microsoft.EntityFrameworkCore;
using OneTravelApi.DataLayer;
using OneTravelApi.EntityLayer;

namespace OneTravelApi.EntityMapping
{
    public class UserMap : IEntityMap
    {
        public void Map(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");

                entity.Property(e => e.UserIdentifer)
                    .IsRequired()
                    .HasMaxLength(268);

                entity.Property(e => e.UserEmail)
                    .HasMaxLength(200);

                entity.Property(e => e.FullName)
                    .HasMaxLength(150);

                entity.Property(e => e.Avatar)
                    .HasMaxLength(350);

                entity.Property(e => e.LastLogin)
                    .IsRequired();

            });
        }
    }
}
