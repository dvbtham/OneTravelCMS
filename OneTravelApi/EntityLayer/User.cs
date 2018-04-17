using System;
using System.Collections.Generic;

namespace OneTravelApi.EntityLayer
{
    public class User
    {
        public int Id { get; set; }

        public string UserIdentifer { get; set; }

        public string UserEmail { get; set; }

        public string FullName { get; set; }

        public string Avatar { get; set; }

        public DateTime LastLogin { get; set; }

        public IEnumerable<Partner> Partners { get; set; } = new HashSet<Partner>();
        public IEnumerable<PartnerContact> PartnerContacts { get; set; } = new HashSet<PartnerContact>();
        public IEnumerable<TravelPrice> TravelPrices { get; set; } = new HashSet<TravelPrice>();
    }
}
