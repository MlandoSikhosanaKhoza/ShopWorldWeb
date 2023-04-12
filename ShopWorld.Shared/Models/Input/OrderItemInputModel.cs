using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.Shared
{
    public class OrderItemInputModel
    {
        public int OrderId { get; set; } 
        public int[] ItemId { get; set; } 
        public int[] Quantity { get; set; }
    }
}
