namespace OneTravelApi.EntityLayer
{
    public class CategoryBooking : IActivable
    {
        public int Id { get; set; }
        public string BookingName { get; set; }
        public int Position { get; set; }
        public bool IsActive { get; set; }
    }
}
