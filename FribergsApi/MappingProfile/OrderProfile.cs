using AutoMapper;
using FribergsApi.Models;

namespace FribergsApi.MappingProfile
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderDto, DAL.Classes.Order>().ReverseMap();
        }
    }

}
