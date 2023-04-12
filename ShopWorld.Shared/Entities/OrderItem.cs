using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ShopWorld.Shared.Entities
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId{get;set;}
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
        [ForeignKey("Item")]
        public int ItemId { get; set; }
        public virtual Item Item { get; set; }
        public int Quantity { get; set; } = 1;
        public decimal Price { get; set; }
    }
}
