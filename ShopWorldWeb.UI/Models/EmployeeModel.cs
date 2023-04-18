using System.ComponentModel.DataAnnotations;

namespace ShopWorldWeb.UI.Models
{
    public class EmployeeModel
    {
        [Key]
        public int EmployeeId { get; set; }
        [Required]
        [MaxLength(40)]
        public string Name { get; set; }
        [Required]
        [MaxLength(40)]
        public string Surname { get; set; }

        public bool IsDeleted { get; set; }=false;
    }
}
