using AutoMapper;
using OneTravelApi.EntityLayer;
using OneTravelApi.Resources;
using OneTravelCMS.Areas.OneOperator.Models;

namespace OneTravelCMS.Core.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CategoryBooking, CategoryBookingViewModel>();
            CreateMap<CategoryBookingViewModel, CategoryBooking>().ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<CategoryBookingStatus, CategoryBookingStatusViewModel>();
            CreateMap<CategoryBookingStatusViewModel, CategoryBookingStatus>().ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<CategoryCity, CategoryCityViewModel>();
            CreateMap<CategoryCityViewModel, CategoryCity>().ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<CategoryLocalTravel, CategoryLocalTravelViewModel>();
            CreateMap<CategoryLocalTravelViewModel, CategoryLocalTravel>().ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<CategoryGroupPartnerResource, CategoryGroupPartnerViewModel>();
            CreateMap<CategoryGroupPartnerViewModel, CategoryGroupPartnerResource>().ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<PartnerResource, PartnerViewModel>();
            CreateMap<PartnerViewModel, PartnerResource>().ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<CategoryPriority, CategoryPriorityViewModel>();
            CreateMap<CategoryPriorityViewModel, CategoryPriority>().ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<CategoryRequest, CategoryRequestViewModel>();
            CreateMap<CategoryRequestViewModel, CategoryRequest>().ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<CategoryRequestStatus, CategoryRequestStatusViewModel>();
            CreateMap<CategoryRequestStatusViewModel, CategoryRequestStatus>().ForMember(x => x.Id, opt => opt.Ignore());
        }
    }
}
