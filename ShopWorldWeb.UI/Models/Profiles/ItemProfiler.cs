using AutoMapper;
using ShopWorld.Shared.Entities;

namespace ShopWorldWeb.UI.Models.Profiles
{
    public class ItemProfiler : Profile
    {
        public ItemProfiler()
        {
            CreateMap<ItemModel, Item>();
            CreateMap<Item, ItemModel>();
        }
    }
}
