using AutoMapper;
using ShopWorld.Shared.Entities;

namespace ShopWorldWeb.UI.Models.Profiles
{
    public class OrderProfiler : Profile
    {
        public OrderProfiler()
        {
            CreateMap<OrderModel, Order>();
            CreateMap<Order, OrderModel>();
        }
    }
}
