using System;

namespace OneTravelApi.EntityLayer
{
    public class TravelRequest
    {
        public int Id { get; set; }

        public int IdCategoryRequest { get; set; }
        public CategoryRequest CategoryRequest { get; set; }

        public int IdCategoryRequestStatus { get; set; }
        public CategoryRequestStatus CategoryRequestStatus { get; set; }

        public int RequestByPartner { get; set; }
        public Partner Partner { get; set; }

        public int RequestByPartnerContact { get; set; }
        public PartnerContact PartnerContact { get; set; }

        public string RequestCode { get; set; }

        public string RequestName { get; set; }

        public string GuestName { get; set; }

        public string GuestMobile { get; set; }

        public string GuestEmail { get; set; }

        public string RequestInfo { get; set; }

        public DateTime UpdateDate { get; set; }

        public int UpdateByUser { get; set; }
        public User User { get; set; }

        public bool IsActive { get; set; }
    }
}
