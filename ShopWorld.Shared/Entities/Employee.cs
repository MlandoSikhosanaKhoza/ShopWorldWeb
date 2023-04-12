using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShopWorld.Shared.Entities
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        [StringLength(40)]
        public string Name { get; set; }
        [StringLength(40)]
        public string Surname { get; set; }
        public bool IsDeleted { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
