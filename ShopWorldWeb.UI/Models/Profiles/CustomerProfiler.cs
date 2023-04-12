using AutoMapper;
using ShopWorld.Shared.Entities;

namespace ShopWorldWeb.UI.Models.Profiles
{
    public class CustomerProfiler : Profile
    {
        public CustomerProfiler()
        {
            CreateMap<CustomerModel, Customer>();
            CreateMap<Customer, CustomerModel>();
        }
    }
}
