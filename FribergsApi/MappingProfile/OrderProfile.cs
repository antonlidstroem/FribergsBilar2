using AutoMapper;
using FribergsApi.Models;

namespace MarcusRent.MappingProfile
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderDto, DAL.Classes.Order>().ReverseMap();
        }
    }

}
