using OneTravelApi.EntityLayer;

namespace OneTravelApi.Resources
{
    public class CategoryBookingResource : IActivable
    {
        public int Id { get; set; }
        public string BookingName { get; set; }
        public bool IsActive { get; set; }
        public int Position { get; set; }
    }
}
