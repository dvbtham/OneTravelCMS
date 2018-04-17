using System;
using System.ComponentModel.DataAnnotations;
using OneTravelApi.Core;

namespace OneTravelApi.Resources
{
    public class TravelPriceResource
    {
        public int Id { get; set; }

        public int IdCategoryLocalTravel { get; set; }
        public KeyValuePairResource CategoryLocalTravel { get; set; }

        public int IdCategoryPrice { get; set; }
        public KeyValuePairResource CategoryPrice { get; set; }

        public string PriceName { get; set; }

        public decimal Price { get; set; }

        public string ContactInfo { get; set; }

        public DateTime UpdateDate { get; set; }

        public int UpdateByUser { get; set; }
        public KeyValuePairResource User { get; set; }

        public bool IsActive { get; set; }
    }

    public class TravelPriceSaveResource
    {
        public int Id { get; set; }

        public int? IdCategoryLocalTravel { get; set; }

        [Required]
        [ForeignRequired]
        public int IdCategoryPrice { get; set; }

        [Required]
        [StringLength(1000)]
        public string PriceName { get; set; }

        [Required]
        public decimal Price { get; set; }

        [StringLength(2000)]
        public string ContactInfo { get; set; }

        public DateTime UpdateDate { get; set; }

        [Required]
        [ForeignRequired]
        public int UpdateByUser { get; set; }

        public bool IsActive { get; set; }
    }
}
