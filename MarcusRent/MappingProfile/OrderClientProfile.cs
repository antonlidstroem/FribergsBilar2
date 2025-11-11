using AutoMapper;
using Fribergs.Core.ViewModels;
using MarcusRent.Models;

namespace MarcusRent.MappingProfile
{
    public class OrderClientProfile : Profile
    {
        public OrderClientProfile()
        {
            CreateMap<OrderDtoClient, OrderViewModel>().ReverseMap();
        }
    }

}
