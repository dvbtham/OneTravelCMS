namespace OneTravelApi.EntityLayer
{
    public class CategoryPriority : IActivable
    {
        public int Id { get; set; }

        public string PriorityName { get; set; }
        public bool IsActive { get; set; }
        public int Position { get; set; }
    }
}
