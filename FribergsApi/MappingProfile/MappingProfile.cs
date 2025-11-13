using AutoMapper;
using DAL.Classes;
using FribergsApi.Models;
using Fribergs.Core.ViewModels;
using System.Linq;
using Fribergs.Core.Models;

namespace MarcusRent.MappingProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // -------------------------
            // ApplicationUser / ApplicationUserDto -> CustomerViewModel
            // -------------------------
            CreateMap<ApplicationUser, CustomerViewModel>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.ApprovedByAdmin, opt => opt.MapFrom(src => src.ApprovedByAdmin));

            CreateMap<ApplicationUserDto, CustomerViewModel>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.ApprovedByAdmin, opt => opt.MapFrom(src => src.ApprovedByAdmin));

            // -------------------------
            // ApplicationUser -> UserDtoClient
            // -------------------------
            CreateMap<ApplicationUser, ApplicationUserDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.ApprovedByAdmin, opt => opt.MapFrom(src => src.ApprovedByAdmin))
                .ForMember(dest => dest.Roles, opt => opt.Ignore());

           
            // -------------------------
            // Login mapping
            // -------------------------
            
            // -------------------------
            // CarDto -> CarViewModel
            // -------------------------
            CreateMap<CarDto, CarViewModel>()
                .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.CarImages.Select(ci => ci.Url)))
                .ForMember(dest => dest.CarDescription, opt => opt.MapFrom(src => src.CarDescription))
                .ForMember(dest => dest.TotalEarnings, opt => opt.Ignore())
                .ForMember(dest => dest.CurrentRentalEndDate, opt => opt.Ignore())
                .ForMember(dest => dest.CurrentCustomerName, opt => opt.Ignore());

            
            // -------------------------
            // CarDto -> OrderViewModel
            // -------------------------
            CreateMap<CarDto, OrderViewModel>()
                .ForMember(dest => dest.CarId, opt => opt.MapFrom(src => src.CarId))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.PricePerDay))
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand))
                .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Model))
                .ForMember(dest => dest.CarDescription, opt => opt.MapFrom(src => src.CarDescription));

            
            // -------------------------
            // OrderDtoClient -> OrderViewModel
            // -------------------------
            CreateMap<Order, OrderViewModel>().ReverseMap();

            // -------------------------
            // CarDto -> CarDto (for CarProfile)
            // -------------------------
            CreateMap<Car, CarDto>()
                .ForMember(dest => dest.CarId, opt => opt.MapFrom(src => src.CarId))
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand))
                .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Model))
                .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Year))
                .ForMember(dest => dest.PricePerDay, opt => opt.MapFrom(src => src.PricePerDay))
                .ForMember(dest => dest.Available, opt => opt.MapFrom(src => src.Available))
                .ForMember(dest => dest.CarDescription, opt => opt.MapFrom(src => src.CarDescription))
                .ForMember(dest => dest.CarImages, opt => opt.MapFrom(src => src.CarImages));

            // -------------------------
            // CarImage -> CarImageDto
            // -------------------------
            CreateMap<CarImage, CarImageDto>()
                .ForMember(dest => dest.CarImageId, opt => opt.MapFrom(src => src.CarImageId))
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url));

            // -------------------------
            // OrderDto -> DAL.Classes.Order
            // -------------------------
            CreateMap<OrderDto, DAL.Classes.Order>().ReverseMap();
        }
    }
}
