using System.ComponentModel.DataAnnotations;

namespace VehiclePartsManagementSystem.Models
{
    public class Part
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Part name is required")]
        [StringLength(100)]
        public string PartName { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, 9999, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 999999, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        /* RELATIONSHIP */

        [Display(Name = "Supplier")]
        public int? SupplierId { get; set; }

        public Supplier? Supplier { get; set; }
    }
}