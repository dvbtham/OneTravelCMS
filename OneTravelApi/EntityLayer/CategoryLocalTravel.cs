﻿using System.Collections.Generic;

namespace OneTravelApi.EntityLayer
{
    public class CategoryLocalTravel
    {
        public int Id { get; set; }
        public int IdCity { get; set; }
        public CategoryCity City { get; set; }

        public string LocalCode { get; set; }
        public string LocalName { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public IEnumerable<TravelPrice> TravelPrices { get; set; } = new List<TravelPrice>();
    }
}
