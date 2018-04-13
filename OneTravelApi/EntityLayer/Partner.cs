using System;
using System.Collections.Generic;

namespace OneTravelApi.EntityLayer
{
    public class Partner
    {
        public int Id { get; set; }

        public int IdGroupPartner { get; set; }
        public CategoryGroupPartner GroupPartner { get; set; }  

        public string TaxCode { get; set; }

        public string PartnerName { get; set; }

        public string Address { get; set; }

        public string Telephone { get; set; }

        public string  Email { get; set; }

        public string Website { get; set; }

        public string Summary { get; set; }

        public DateTime UpdateDate { get; set; }

        public int UpdateByUser { get; set; }
        public User User { get; set; }

        public bool IsActive { get; set; }

        public IEnumerable<PartnerContact> PartnerContacts { get; set; } = new List<PartnerContact>();
    }
}
