using Microsoft.EntityFrameworkCore;

namespace OneTravelApi.DataLayer
{
    public interface IEntityMap
    {
        void Map(ModelBuilder modelBuilder);
    }
}
