using AutoMapper;
using DAL.Classes;  // För ApplicationUser
using FribergsApi.Models;  // För ApplicationUserDto

namespace FribergsApi.MappingProfile
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // Mappa från ApplicationUser till ApplicationUserDto
            CreateMap<ApplicationUser, ApplicationUserDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.ApprovedByAdmin, opt => opt.MapFrom(src => src.ApprovedByAdmin));

            // Mappa från ApplicationUserDto till ApplicationUser
            CreateMap<ApplicationUserDto, ApplicationUser>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => GetFirstName(src.FullName)))  
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => GetLastName(src.FullName)))    
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.ApprovedByAdmin, opt => opt.MapFrom(src => src.ApprovedByAdmin));
        }

        // Hjälpmetod för att extrahera förnamn från FullName
        private string GetFirstName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName)) return string.Empty;
            var nameParts = fullName.Split(' ');
            return nameParts.FirstOrDefault() ?? string.Empty; 
        }

        // Hjälpmetod för att extrahera efternamn från FullName
        private string GetLastName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName)) return string.Empty;
            var nameParts = fullName.Split(' ');
            return nameParts.Length > 1 ? string.Join(" ", nameParts.Skip(1)) : string.Empty; 
        }
    }
}
