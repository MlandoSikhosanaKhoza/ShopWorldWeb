using AutoMapper;
using ShopWorld.Shared;
using ShopWorld.Shared.Entities;

namespace ShopWorldWeb.UI.Models.Profiles
{
    public class ItemProfiler : Profile
    {
        public ItemProfiler()
        {
            CreateMap<ItemModel, ItemInputModel>();
            CreateMap<Item, ItemModel>();
        }
    }
}
