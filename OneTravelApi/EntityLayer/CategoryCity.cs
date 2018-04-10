using System.Collections.Generic;

namespace OneTravelApi.EntityLayer
{
    public class CategoryCity : IActivable
    {
        public int Id { get; set; }
        public string CityCode { get; set; }
        public string CityName { get; set; }
        public bool IsActive { get; set; }
        public int Position { get; set; }

        public IEnumerable<CategoryLocalTravel> LocalTravels { get; set; } = new List<CategoryLocalTravel>();
    }
}
