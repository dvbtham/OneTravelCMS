using System;

namespace OneTravelApi.EntityLayer
{
    public class TravelPrice
    {
        public int Id { get; set; }

        public int? IdCategoryLocalTravel { get; set; }
        public CategoryLocalTravel CategoryLocalTravel { get; set; }

        public int IdCategoryPrice { get; set; }
        public CategoryPrice CategoryPrice { get; set; }

        public string PriceName { get; set; }

        public decimal Price { get; set; }

        public string ContactInfo { get; set; }

        public DateTime UpdateDate { get; set; }

        public int UpdateByUser { get; set; }
        public User User { get; set; }

        public bool IsActive { get; set; }
    }
}
