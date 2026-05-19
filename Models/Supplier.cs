using System.ComponentModel.DataAnnotations;

namespace VehiclePartsManagementSystem.Models
{
    public class Supplier
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Supplier name is required")]
        [StringLength(100)]
        public string SupplierName { get; set; }

        [Required(ErrorMessage = "Contact number is required")]
        [Phone(ErrorMessage = "Enter a valid phone number")]
        public string ContactNumber { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(200)]
        public string Address { get; set; }

        /* RELATIONSHIP */

        public ICollection<Part>? Parts { get; set; }
    }
}