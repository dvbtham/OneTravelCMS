using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OneTravelApi.Resources
{
    public class CategoryPriceResource
    {
        public int Id { get; set; }

        [Required]
        [StringLength(650)]
        public string CategoryPriceName { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public int Position { get; set; } = 1;

        public IEnumerable<KeyValuePairResource> TravelPrices { get; set; } = new List<KeyValuePairResource>();
    }

    public class CategoryPriceSaveResource
    {
        public int Id { get; set; }

        [Required]
        [StringLength(650)]
        public string CategoryPriceName { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public int Position { get; set; } = 1;
    }
}
