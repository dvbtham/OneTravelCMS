namespace OneTravelApi.EntityLayer
{
    public interface IActivable
    {
        bool IsActive { get; set; }
        int Position { get; set; }
    }
}
