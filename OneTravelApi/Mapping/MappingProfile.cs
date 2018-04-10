using AutoMapper;
using OneTravelApi.EntityLayer;
using OneTravelApi.Resources;

namespace OneTravelApi.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CategoryBooking, CategoryBooking>();
            CreateMap<CategoryBookingStatus, CategoryBookingStatus>();
            CreateMap<CategoryCity, CategoryCityResource>();
            CreateMap<CategoryCityResource, CategoryCity>().ForMember(x => x.Id, opt => opt.Ignore());
        }
    }
}
