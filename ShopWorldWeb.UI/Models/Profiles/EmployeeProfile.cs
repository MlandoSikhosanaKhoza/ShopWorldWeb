using AutoMapper;
using ShopWorld.Shared.Entities;

namespace ShopWorldWeb.UI.Models.Profiles
{
    public class EmployeeProfile:Profile
    {
        public EmployeeProfile() {
            CreateMap<EmployeeModel, Employee>();
            CreateMap<Employee, EmployeeModel>();
        }
    }
}
