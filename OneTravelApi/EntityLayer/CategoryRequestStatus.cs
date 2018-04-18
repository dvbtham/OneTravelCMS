using System.Collections.Generic;

namespace OneTravelApi.EntityLayer
{
    public class CategoryRequestStatus : IActivable
    {
        public int Id { get; set; }
        public string RequestStatusCode { get; set; }
        public string RequestStatusName { get; set; }
        public bool IsActive { get; set; }
        public int Position { get; set; }

        public IEnumerable<TravelRequest> TravelRequests { get; set; } = new List<TravelRequest>();
    }
}
