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
                .ForMember(dest => dest.ApprovedByAdmin, opt => opt.MapFrom(src => src.ApprovedByAdmin));

            CreateMap<CustomerViewModel, ApplicationUser>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.FirstName, opt => opt.Ignore())
                .ForMember(dest => dest.LastName, opt => opt.Ignore())
                .ForMember(dest => dest.FullName, opt => opt.Ignore())
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .AfterMap((src, dest) =>
                {
                    // För- och efternamn från FullName
                    var parts = src.FullName.Split(' ', 2);
                    dest.FirstName = parts[0];
                    dest.LastName = parts.Length > 1 ? parts[1] : "";
                });

            CreateMap<UserDto, CustomerViewModel>()
             .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
             .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
             .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
             .ForMember(dest => dest.ApprovedByAdmin, opt => opt.MapFrom(src => src.ApprovedByAdmin));



            CreateMap<CarDto, CarViewModel>()
                .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.CarImages.Select(ci => ci.Url)))
                .ForMember(dest => dest.CarDescription, opt => opt.MapFrom(src => src.CarDescription))
                .ForMember(dest => dest.TotalEarnings, opt => opt.Ignore())
                .ForMember(dest => dest.CurrentRentalEndDate, opt => opt.Ignore())
                .ForMember(dest => dest.CurrentCustomerName, opt => opt.Ignore()).ReverseMap(); ;


            CreateMap<CarDto, OrderViewModel>()
                .ForMember(dest => dest.CarId, opt => opt.MapFrom(src => src.CarId))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.PricePerDay));




            CreateMap<Order, OrderViewModel>()
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Car.Brand))
                .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Car.Model))
                .ForMember(dest => dest.CarDescription, opt => opt.MapFrom(src => src.Car.CarDescription))
                .ReverseMap(); // ReverseMap för POST/PUT behövs inte för dessa fält

            CreateMap<OrderDto, Order>().ReverseMap();


            CreateMap<OrderViewModel, OrderDto>()
                .ForMember(dest => dest.CarId, opt => opt.MapFrom(src => src.CarId))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand))
                .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Model))
                .ForMember(dest => dest.CarDescription, opt => opt.MapFrom(src => src.CarDescription))
                .ReverseMap();





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
