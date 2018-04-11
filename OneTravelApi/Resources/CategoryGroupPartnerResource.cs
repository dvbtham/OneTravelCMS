namespace OneTravelApi.Resources
{
    public class CategoryGroupPartnerResource
    {
        public int Id { get; set; }
        public string GroupPartnerCode { get; set; }
        public string GroupPartnerName { get; set; }
        public bool IsLocal { get; set; }
        public bool IsActive { get; set; }
        public int Position { get; set; }
    }
}
