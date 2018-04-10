using System.Collections.Generic;

namespace OneTravelApi.EntityLayer
{
    public class CategoryGroupPartner : IActivable
    {
        public int Id { get; set; }
        public string GroupPartnerCode { get; set; }
        public string GroupPartnerName { get; set; }
        public bool IsLocal { get; set; }
        public bool IsActive { get; set; }
        public int Position { get; set; }

        public IEnumerable<Partner> Partners { get; set; } = new HashSet<Partner>();
    }
}
