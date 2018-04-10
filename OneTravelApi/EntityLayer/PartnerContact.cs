using System;

namespace OneTravelApi.EntityLayer
{
    public class PartnerContact
    {
        public int Id { get; set; }

        public int IdPartner { get; set; }
        public Partner Partner { get; set; }

        public string ContactName { get; set; }

        public string PositionTitle { get; set; }

        public string Mobile { get; set; }

        public string  Email { get; set; }

        public string Note { get; set; }

        public DateTime UpdateDate { get; set; }

        public int UpdateByUser { get; set; }
        public User User { get; set; }

        public bool IsActive { get; set; }
    }
}
