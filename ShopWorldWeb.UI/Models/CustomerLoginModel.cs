using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ShopWorldWeb.UI.Models
{
    public class CustomerLoginModel
    {
        [Required]
        [MaxLength(11)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Only numbers allowed")]
        [Remote("CheckIfMobileDoesntExist", "Customer", ErrorMessage = "Mobile number doesnt exist")]
        public string Mobile { get; set; }
    }
}
