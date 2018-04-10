using System.Collections.Generic;
using OneTravelApi.EntityMapping;

namespace OneTravelApi.DataLayer
{
    public class OneTravelEntityMapper : EntityMapper
    {
        public OneTravelEntityMapper()
        {
            Mappings = new List<IEntityMap>()
            {
                new CategoryBookingMap(),
                new CategoryBookingStatusMap(),
                new CategoryCityMap(),
                new CategoryGroupPartnerMap(),
                new CategoryLocalTravelMap(),
                new CategoryPriorityMap(),
                new CategoryRequestMap(),
                new CategoryRequestStatusMap(),
                new PartnerContactMap(),
                new PartnerMap(),
                new UserMap()
            };
        }
    }
}
