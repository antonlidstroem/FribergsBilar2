using AutoMapper;
using DAL.Classes;
using Fribergs.Core.ViewModels;
using Fribergs.Core.DTO;
using System.Linq;

namespace FribergsApi.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // -------------------------
            // ApplicationUser → CustomerViewModel
            // -------------------------
            CreateMap<ApplicationUser, CustomerViewModel>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.ApprovedByAdmin, opt => opt.MapFrom(src => src.ApprovedByAdmin));

            // -------------------------
            // ApplicationUser → UserDto
            // -------------------------
            CreateMap<ApplicationUser, UserDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.ApprovedByAdmin, opt => opt.MapFrom(src => src.ApprovedByAdmin))
                .ForMember(dest => dest.Roles, opt => opt.Ignore());

            // -------------------------
            // UserDto → ApplicationUser
            // -------------------------
            CreateMap<UserDto, ApplicationUser>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.ApprovedByAdmin, opt => opt.MapFrom(src => src.ApprovedByAdmin))
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.SecurityStamp, opt => opt.Ignore())
                .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());

            // -------------------------
            // CustomerViewModel → ApplicationUser
            // -------------------------
            CreateMap<CustomerViewModel, ApplicationUser>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.ApprovedByAdmin, opt => opt.MapFrom(src => src.ApprovedByAdmin));

            // -------------------------
            // UserDto ↔ CustomerViewModel
            // -------------------------
            CreateMap<UserDto, CustomerViewModel>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.ApprovedByAdmin, opt => opt.MapFrom(src => src.ApprovedByAdmin));

            CreateMap<CustomerViewModel, UserDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.ApprovedByAdmin, opt => opt.MapFrom(src => src.ApprovedByAdmin))
                .ForMember(dest => dest.Roles, opt => opt.Ignore())
                .ForMember(dest => dest.Password, opt => opt.Ignore()); // CustomerViewModel har inget password


            // -------------------------
            // CarDto ↔ CarViewModel
            // -------------------------
            CreateMap<CarDto, CarViewModel>()
                .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.CarImages.Select(ci => ci.Url)))
                .ForMember(dest => dest.CarDescription, opt => opt.MapFrom(src => src.CarDescription))
                .ForMember(dest => dest.TotalEarnings, opt => opt.Ignore())
                .ForMember(dest => dest.CurrentRentalEndDate, opt => opt.Ignore())
                .ForMember(dest => dest.CurrentCustomerName, opt => opt.Ignore());

            CreateMap<CarViewModel, CarDto>()
                .ForMember(dest => dest.CarId, opt => opt.MapFrom(src => src.CarId))
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand))
                .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Model))
                .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Year))
                .ForMember(dest => dest.PricePerDay, opt => opt.MapFrom(src => src.PricePerDay))
                .ForMember(dest => dest.Available, opt => opt.MapFrom(src => src.Available))
                .ForMember(dest => dest.CarDescription, opt => opt.MapFrom(src => src.CarDescription))
                .ForMember(dest => dest.CarImagesResponse, opt => opt.MapFrom(src =>
                    new CarImageResponse
                    {
                        Values = src.ImageUrls
                            .Where(u => !string.IsNullOrEmpty(u))
                            .Select(url => new CarImageDto { Url = url })
                            .ToList()
                    }));

            // -------------------------
            // Order mappings
            // -------------------------
            CreateMap<CarDto, OrderViewModel>()
                .ForMember(dest => dest.CarId, opt => opt.MapFrom(src => src.CarId))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.PricePerDay));

            CreateMap<Order, OrderViewModel>()
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Car.Brand))
                .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Car.Model))
                .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Car.Year))
                .ForMember(dest => dest.CarDescription, opt => opt.MapFrom(src => src.Car.CarDescription));

            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Car != null ? src.Car.Brand : null))
                .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Car != null ? src.Car.Model : null))
                .ForMember(dest => dest.CarDescription, opt => opt.MapFrom(src => src.Car != null ? src.Car.CarDescription : null));

            CreateMap<OrderDto, Order>();
            CreateMap<OrderDto, OrderViewModel>().ReverseMap();

            // -------------------------
            // Car ↔ CarDto
            // -------------------------
            CreateMap<Car, CarDto>()
                .ForMember(dest => dest.CarImages, opt => opt.MapFrom(src => src.CarImages))
                .ReverseMap();

            // -------------------------
            // CarImage ↔ CarImageDto
            // -------------------------
            CreateMap<CarImage, CarImageDto>().ReverseMap();
        }
    }
}
