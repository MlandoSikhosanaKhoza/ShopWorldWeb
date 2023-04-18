using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.Shared
{
    public class ItemInputModel
    {
        [Key]
        public int ItemId { get; set; }
        public string ImageName { get; set; }
        public string Base64 { get; set; }
        [StringLength(40)]
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsDeleted { get; set; }
    }
}
