using AutoMapper;
using DAL.Classes;
using Fribergs.Core.ViewModels;
using System.Linq;
using Fribergs.Core.DTO;

namespace FribergsApi.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, CustomerViewModel>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.ApprovedByAdmin, opt => opt.MapFrom(src => src.ApprovedByAdmin))
                .ReverseMap(); ;

            CreateMap<ApplicationUserDto, CustomerViewModel>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.ApprovedByAdmin, opt => opt.MapFrom(src => src.ApprovedByAdmin))
                .ReverseMap();

            CreateMap<ApplicationUser, ApplicationUserDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.ApprovedByAdmin, opt => opt.MapFrom(src => src.ApprovedByAdmin))
                .ForMember(dest => dest.Roles, opt => opt.Ignore()).ReverseMap();

           
            CreateMap<CarDto, CarViewModel>()
                .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.CarImages.Select(ci => ci.Url)))
                .ForMember(dest => dest.CarDescription, opt => opt.MapFrom(src => src.CarDescription))
                .ForMember(dest => dest.TotalEarnings, opt => opt.Ignore())
                .ForMember(dest => dest.CurrentRentalEndDate, opt => opt.Ignore())
                .ForMember(dest => dest.CurrentCustomerName, opt => opt.Ignore()).ReverseMap(); ;

            
            CreateMap<CarDto, OrderViewModel>()
                .ForMember(dest => dest.CarId, opt => opt.MapFrom(src => src.CarId))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.PricePerDay))
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand))
                .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Model))
                .ForMember(dest => dest.CarDescription, opt => opt.MapFrom(src => src.CarDescription))
                .ReverseMap(); ;

            
           
            CreateMap<Order, OrderViewModel>().ReverseMap();
            CreateMap<OrderDto, Order>().ReverseMap();
            CreateMap<OrderDto, OrderViewModel>().ReverseMap();

            CreateMap<Car, CarDto>()
                .ForMember(dest => dest.CarId, opt => opt.MapFrom(src => src.CarId))
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand))
                .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Model))
                .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Year))
                .ForMember(dest => dest.PricePerDay, opt => opt.MapFrom(src => src.PricePerDay))
                .ForMember(dest => dest.Available, opt => opt.MapFrom(src => src.Available))
                .ForMember(dest => dest.CarDescription, opt => opt.MapFrom(src => src.CarDescription))
                .ForMember(dest => dest.CarImages, opt => opt.MapFrom(src => src.CarImages))
                .ReverseMap(); ;

   
            CreateMap<CarImage, CarImageDto>()
                .ForMember(dest => dest.CarImageId, opt => opt.MapFrom(src => src.CarImageId))
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url))
                .ReverseMap(); ;

        
            
        }
    }
}
