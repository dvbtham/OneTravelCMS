using System.Collections.Generic;

namespace OneTravelApi.EntityLayer
{
    public class CategoryPrice : IActivable
    {
        public int Id { get; set; }

        public string CategoryPriceName { get; set; }

        public bool IsActive { get; set; }
        public int Position { get; set; }

        public IEnumerable<TravelPrice> TravelPrices { get; set; } = new List<TravelPrice>();
    }
}
