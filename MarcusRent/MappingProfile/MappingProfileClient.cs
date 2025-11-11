using AutoMapper;
using Fribergs.Core.ViewModels;
using FribergsApi.Models;
using MarcusRent.Models;
using System.Linq;

namespace MarcusRent.MappingProfile
{
    public class MappingProfileClient : Profile
    {
        public MappingProfileClient()
        {
            // -------------------------
            // Car mapping
            // -------------------------
            CreateMap<CarDto, CarDtoClient>()
                .ForMember(dest => dest.CarImagesResponse,
                           opt => opt.MapFrom(src => new CarImageResponseClient
                           {
                               Values = src.CarImages.Select(ci => new CarImageDtoClient
                               {
                                   CarImageId = ci.CarImageId,
                                   Url = ci.Url,
                                   CarId = ci.CarId
                               }).ToList()
                           }))
                .ForMember(dest => dest.CarImages, opt => opt.Ignore());

            // -------------------------
            // Order mapping
            // -------------------------
            CreateMap<OrderDto, OrderDtoClient>()
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand))
                .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Model))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.UserId));

            // -------------------------
            // User mapping
            // -------------------------
            CreateMap<ApplicationUserDto, UserDtoClient>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Roles, opt => opt.Ignore());

            // -------------------------
            // Login mapping
            // -------------------------
            CreateMap<LoginUserDto, LoginUserDtoClient>();

            // -------------------------
            // OrderViewModel mapping
            // -------------------------
            CreateMap<ApplicationUserDto, OrderViewModel>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.FullName));

            CreateMap<CarDto, OrderViewModel>()
                .ForMember(dest => dest.CarId, opt => opt.MapFrom(src => src.CarId))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.PricePerDay))
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand))
                .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Model))
                .ForMember(dest => dest.CarDescription, opt => opt.MapFrom(src => src.CarDescription));

            CreateMap<CarDto, CarViewModel>()
                .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.CarImages.Select(ci => ci.Url)))
                .ForMember(dest => dest.CarDescription, opt => opt.MapFrom(src => src.CarDescription))
                .ForMember(dest => dest.TotalEarnings, opt => opt.Ignore())
                .ForMember(dest => dest.CurrentRentalEndDate, opt => opt.Ignore())
                .ForMember(dest => dest.CurrentCustomerName, opt => opt.Ignore());

            CreateMap<ApplicationUserDto, CustomerViewModel>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.ApprovedByAdmin, opt => opt.MapFrom(src => src.ApprovedByAdmin));

            CreateMap<CarDtoClient, CarViewModel>()
                .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.CarImages.Select(ci => ci.Url)))
                .ForMember(dest => dest.CarDescription, opt => opt.MapFrom(src => src.CarDescription))
                .ForMember(dest => dest.TotalEarnings, opt => opt.Ignore())
                .ForMember(dest => dest.CurrentRentalEndDate, opt => opt.Ignore())
                .ForMember(dest => dest.CurrentCustomerName, opt => opt.Ignore());

            CreateMap<CarDtoClient, OrderViewModel>()
                .ForMember(dest => dest.CarId, opt => opt.MapFrom(src => src.CarId))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.PricePerDay))
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand))
                .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Model))
                .ForMember(dest => dest.CarDescription, opt => opt.MapFrom(src => src.CarDescription));



        }
    }
}
