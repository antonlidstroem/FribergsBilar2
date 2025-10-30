using AutoMapper;
using DAL.Classes;
using FribergsApi.Models;

namespace FribergsApi.MappingProfile
{
    public class CarProfile : Profile
    {
        public CarProfile()
        {
            // Mappa från Car till CarDto
            CreateMap<Car, CarDto>()
                .ForMember(dest => dest.CarId, opt => opt.MapFrom(src => src.CarId))
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand))
                .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Model))
                .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Year))
                .ForMember(dest => dest.PricePerDay, opt => opt.MapFrom(src => src.PricePerDay))
                .ForMember(dest => dest.Available, opt => opt.MapFrom(src => src.Available))
                .ForMember(dest => dest.CarDescription, opt => opt.MapFrom(src => src.CarDescription))

                // Mappa CarImages från Car till CarDto som en lista av CarImageDto
                .ForMember(dest => dest.CarImages, opt => opt.MapFrom(src => src.CarImages));  

            // Mappa från CarImage till CarImageDto
            CreateMap<CarImage, CarImageDto>()
                .ForMember(dest => dest.CarImageId, opt => opt.MapFrom(src => src.CarImageId))
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url)); 
        }
    }
}

