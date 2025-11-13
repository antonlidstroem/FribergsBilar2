using AutoMapper;
using Fribergs.Core.Models;
using Fribergs.Core.ViewModels;


namespace MarcusRent.MappingProfile
{
    public class OrderClientProfile : Profile
    {
        public OrderClientProfile()
        {
            CreateMap<OrderDto, OrderViewModel>().ReverseMap();
        }
    }

}
