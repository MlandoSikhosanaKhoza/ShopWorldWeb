using ShopWorld.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ShopWorld.Shared.Entities
{
    public class Order
    {
        [Key]
        public int OrderId{ get;set; }
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        [ForeignKey("Employee")]
        public int? EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
        public DateTime? DateFulfilled { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid OrderReference { get; set; }
        public decimal VAT { get; set; } = Tax.VAT;
        public decimal Subtotal { get; set; }
        public decimal GrandTotal { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
