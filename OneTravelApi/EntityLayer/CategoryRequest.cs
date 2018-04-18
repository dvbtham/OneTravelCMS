using System.Collections.Generic;

namespace OneTravelApi.EntityLayer
{
    public class CategoryRequest : IActivable
    {
        public int Id { get; set; }

        public string RequestName { get; set; }
        public bool IsActive { get; set; }
        public int Position { get; set; }

        public IEnumerable<TravelRequest> TravelRequests { get; set; } = new List<TravelRequest>();
    }
}
