using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ShopWorldWeb.UI.Models
{
    public class ItemModel
    {
        public int ItemId { get; set; }
        [Required]
        [MaxLength(40)]
        [Display(Name = "Item name")]
        public string Description { get; set; }
        [StringLength(300)]
        public string ImageName { get; set; }
        public string Base64 { get; set; }
        [Required]
        [Display(Name = "Price without VAT")]
        public decimal Price { get; set; }
    }
}
