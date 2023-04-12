using Microsoft.AspNetCore.Mvc;
using ShopWorld.Shared.Entities;
using System.ComponentModel.DataAnnotations;

namespace ShopWorldWeb.UI.Models
{
    public class CustomerModel
    {
        public int CustomerId { get; set; }
        [Required]
        [MaxLength(40)]
        public string Name { get; set; }
        [Required]
        [MaxLength(40)]
        public string Surname { get; set; }
        [Required]
        [MaxLength(11)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Only numbers allowed")]
        [Remote("CheckIfMobileExist", "Customer", ErrorMessage = "Mobile number already exists", AdditionalFields = "CustomerId")]
        public string Mobile { get; set; }
    }
    public class CustomerHistoryModel : CustomerModel
    {
        public List<Order> OngoingOrders { get; set; }
        public List<Order> CompleteOrders { get; set; }
    }
}
