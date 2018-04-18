using System.Linq;
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

            CreateMap<CategoryPrice, CategoryPriceResource>()
                .ForMember(x => x.TravelPrices,
                opt => opt.MapFrom(x => x.TravelPrices.Select(tp => new KeyValuePairResource
                {
                    Id = tp.Id,
                    Name = tp.PriceName
                }
                )));

            CreateMap<CategoryPriceSaveResource, CategoryPrice>()
                .ForMember(x => x.Id, opt => opt.Ignore());

            #region TravelPrice

            CreateMap<TravelPrice, TravelPriceResource>()
                .ForMember(x => x.CategoryLocalTravel,
                    opt => opt.MapFrom(x =>
                        new KeyValuePairResource
                        {
                            Id = x.CategoryLocalTravel.Id,
                            Name = x.CategoryLocalTravel.LocalName
                        }))
                .ForMember(x => x.CategoryPrice,
                    opt => opt.MapFrom(x =>
                        new KeyValuePairResource
                        {
                            Id = x.CategoryPrice.Id,
                            Name = x.CategoryPrice.CategoryPriceName
                        }))
                .ForMember(x => x.User,
                    opt => opt.MapFrom(x =>
                        new KeyValuePairResource
                        {
                            Id = x.User.Id,
                            Name = x.User.FullName
                        }));

            CreateMap<TravelPriceSaveResource, TravelPrice>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .AfterMap((resource, entity) =>
                {
                    if (resource.IdCategoryLocalTravel == 0) entity.IdCategoryLocalTravel = null;
                });

            #endregion

            #region TravelRequest

            CreateMap<TravelRequestSaveResource, TravelRequest>().ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<TravelRequest, TravelRequestResource>()
                .ForMember(x => x.CategoryRequest,
                    opt => opt.MapFrom(x =>
                        new KeyValuePairResource
                        {
                            Id = x.CategoryRequest.Id,
                            Name = x.CategoryRequest.RequestName
                        }))
                .ForMember(x => x.CategoryRequestStatus,
                    opt => opt.MapFrom(x =>
                        new KeyValuePairResource
                        {
                            Id = x.CategoryRequestStatus.Id,
                            Name = x.CategoryRequestStatus.RequestStatusName
                        }))
                .ForMember(x => x.Partner,
                    opt => opt.MapFrom(x =>
                        new KeyValuePairResource
                        {
                            Id = x.Partner.Id,
                            Name = x.Partner.PartnerName
                        }))
                .ForMember(x => x.PartnerContact,
                    opt => opt.MapFrom(x =>
                        new KeyValuePairResource
                        {
                            Id = x.PartnerContact.Id,
                            Name = x.PartnerContact.ContactName
                        }))
                .ForMember(x => x.User,
                    opt => opt.MapFrom(x =>
                        new KeyValuePairResource
                        {
                            Id = x.User.Id,
                            Name = x.User.FullName
                        }));

            CreateMap<TravelPriceSaveResource, TravelPrice>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .AfterMap((resource, entity) =>
                {
                    if (resource.IdCategoryLocalTravel == 0) entity.IdCategoryLocalTravel = null;
                });

            #endregion

            #region Partner

            CreateMap<Partner, PartnerResource>();
            CreateMap<PartnerResource, Partner>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.PartnerContacts, opt => opt.Ignore());

            CreateMap<PartnerContact, PartnerContactResource>();
            CreateMap<PartnerContactResource, PartnerContact>().ForMember(x => x.Id, opt => opt.Ignore());

            #endregion

        }
    }
}
