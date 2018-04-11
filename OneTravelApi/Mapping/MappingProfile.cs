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
            CreateMap<CategoryGroupPartnerResource, CategoryGroupPartner>().ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<CategoryGroupPartner, CategoryGroupPartnerResource>();
            CreateMap<CategoryLocalTravel, CategoryLocalTravel>();
            CreateMap<CategoryRequest, CategoryRequest>();
            CreateMap<CategoryRequestStatus, CategoryRequestStatus>();
            CreateMap<CategoryPriority, CategoryPriority>();

            CreateMap<Partner, PartnerResource>();
            CreateMap<PartnerResource, Partner>().ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<PartnerContact, PartnerContactResource > ();
            CreateMap<PartnerContactResource, PartnerContact>().ForMember(x => x.Id, opt => opt.Ignore());
        }
    }
}
