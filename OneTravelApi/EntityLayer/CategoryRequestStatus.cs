namespace OneTravelApi.EntityLayer
{
    public class CategoryRequestStatus : IActivable
    {
        public int Id { get; set; }
        public string RequestStatusCode { get; set; }
        public string RequestStatusName { get; set; }
        public bool IsActive { get; set; }
        public int Position { get; set; }
    }
}
