namespace OneTravelApi.EntityLayer
{
    public class CategoryRequest : IActivable
    {
        public int Id { get; set; }

        public string RequestName { get; set; }
        public bool IsActive { get; set; }
        public int Position { get; set; }
    }
}
