using System;
using System.ComponentModel.DataAnnotations;

namespace OneTravelApi.Resources
{
    public class TravelRequestResource
    {
        public int Id { get; set; }

        public int IdCategoryRequest { get; set; }
        public KeyValuePairResource CategoryRequest { get; set; }

        public int IdCategoryRequestStatus { get; set; }
        public KeyValuePairResource CategoryRequestStatus { get; set; }

        public int RequestByPartner { get; set; }
        public KeyValuePairResource Partner { get; set; }

        public int RequestByPartnerContact { get; set; }
        public KeyValuePairResource PartnerContact { get; set; }

        public string RequestCode { get; set; }

        public string RequestName { get; set; }

        public string GuestName { get; set; }

        public string GuestMobile { get; set; }

        public string GuestEmail { get; set; }

        public string RequestInfo { get; set; }

        public DateTime UpdateDate { get; set; }

        public int UpdateByUser { get; set; }
        public KeyValuePairResource User { get; set; }

        public bool IsActive { get; set; }
    }

    public class TravelRequestSaveResource
    {
        public int Id { get; set; }

        [Required]
        public int IdCategoryRequest { get; set; }

        [Required]
        public int IdCategoryRequestStatus { get; set; }

        [Required]
        public int RequestByPartner { get; set; }

        [Required]
        public int RequestByPartnerContact { get; set; }

        [Required]
        [StringLength(50)]
        public string RequestCode { get; set; }

        [Required]
        [StringLength(500)]
        public string RequestName { get; set; }

        [StringLength(250)]
        public string GuestName { get; set; }

        [StringLength(80)]
        public string GuestMobile { get; set; }

        [StringLength(250)]
        public string GuestEmail { get; set; }

        public string RequestInfo { get; set; }

        public DateTime UpdateDate { get; set; } = DateTime.Today;

        public int UpdateByUser { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
