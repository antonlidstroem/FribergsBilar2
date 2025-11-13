using AutoMapper;
using DAL.Classes;
using FribergsApi.Models;
using Fribergs.Core.ViewModels;
using System.Linq;
using Fribergs.Core.Models;

namespace MarcusRent.MappingProfile
{
    public class MappingProfileClient : Profile
    {
        public MappingProfileClient()
        {
            //// -------------------------
            //// ApplicationUser / ApplicationUserDto -> CustomerViewModel
            //// -------------------------
            //CreateMap<ApplicationUser, CustomerViewModel>()
            //    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
            //    .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
            //    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            //    .ForMember(dest => dest.ApprovedByAdmin, opt => opt.MapFrom(src => src.ApprovedByAdmin));

            //CreateMap<ApplicationUserDto, CustomerViewModel>()
            //    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
            //    .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            //    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            //    .ForMember(dest => dest.ApprovedByAdmin, opt => opt.MapFrom(src => src.ApprovedByAdmin));

            //// -------------------------
            //// ApplicationUserDto -> UserDtoClient
            //// -------------------------
            //CreateMap<ApplicationUserDto, UserDtoClient>()
            //    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
            //    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            //    .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            //    .ForMember(dest => dest.ApprovedByAdmin, opt => opt.MapFrom(src => src.ApprovedByAdmin))
            //    .ForMember(dest => dest.Roles, opt => opt.Ignore()); // You might map roles separately

            //// -------------------------
            //// ApplicationUser -> UserDtoClient
            //// -------------------------
            //CreateMap<ApplicationUser, UserDtoClient>()
            //    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
            //    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            //    .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
            //    .ForMember(dest => dest.ApprovedByAdmin, opt => opt.MapFrom(src => src.ApprovedByAdmin))
            //    .ForMember(dest => dest.Roles, opt => opt.Ignore()); // Roles are often handled separately

            // -------------------------
            // Login mapping
            // -------------------------
            CreateMap<LoginUserDto, LoginUserDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));

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
            //// ApplicationUserDto -> OrderViewModel
            //// -------------------------
            //CreateMap<ApplicationUserDto, OrderViewModel>()
            //    .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.FullName));

            // -------------------------
            // CarDtoClient -> OrderViewModel
            // -------------------------
            CreateMap<CarDto, OrderViewModel>()
                .ForMember(dest => dest.CarId, opt => opt.MapFrom(src => src.CarId))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.PricePerDay))
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand))
                .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Model))
                .ForMember(dest => dest.CarDescription, opt => opt.MapFrom(src => src.CarDescription));

            // -------------------------
            //// ApplicationUser -> CustomerViewModel
            //// -------------------------
            //CreateMap<ApplicationUser, CustomerViewModel>()
            //    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
            //    .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
            //    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            //    .ForMember(dest => dest.ApprovedByAdmin, opt => opt.MapFrom(src => src.ApprovedByAdmin));

            // -------------------------
            // Other DTO and ViewModel mappings (if needed)
            // -------------------------
            // Add more mappings as needed for different entities such as Orders, Cars, etc.
        }
    }
}
