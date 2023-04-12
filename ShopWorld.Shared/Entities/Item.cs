using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShopWorld.Shared.Entities
{
    public class Item
    {
        [Key]
        public int ItemId { get; set; }
        [StringLength(300)]
        public string ImageName { get; set; }
        [StringLength(40)]
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
