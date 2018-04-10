using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace OneTravelApi.DataLayer
{
    public class OneTravelDbContext : DbContext
    {
        public OneTravelDbContext(IOptions<AppSettings> appSettings, IEntityMapper entityMapper)
        {
            ConnectionString = appSettings.Value.ConnectionString;

            EntityMapper = entityMapper;
        }

        public string ConnectionString { get; }

        public IEntityMapper EntityMapper { get; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString, paging => paging.UseRowNumberForPaging());

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            EntityMapper.MapEntities(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
    }
}
