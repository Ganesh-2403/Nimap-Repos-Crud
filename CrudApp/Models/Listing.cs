using System.ComponentModel.DataAnnotations;

namespace CrudApp.Models
{
    public class Listing
    {
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product Name is required")]
        [StringLength(255)]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }
    }
}
