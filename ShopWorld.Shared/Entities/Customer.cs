using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShopWorld.Shared.Entities
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        [StringLength(40)]
        public string Name { get; set; }
        [StringLength(40)]
        public string Surname { get; set; }
        [StringLength(11)]
        public string Mobile { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
