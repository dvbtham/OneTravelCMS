namespace OneTravelApi.EntityLayer
{
    public class CategoryBookingStatus : IActivable
    {
        public int Id { get; set; }

        public string BookingStatusCode { get; set; }

        public string BookingStatusName { get; set; }

        public bool IsActive { get; set; }
        public int Position { get; set; }
    }
}
