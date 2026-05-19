using System.ComponentModel.DataAnnotations;

namespace VehiclePartsManagementSystem.Models
{
    public class Customer
    {
        public int Id { get; set; }

        /* CUSTOMER NAME */

        [Required]
        [StringLength(100)]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

        /* EMAIL */

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /* PASSWORD */

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        /* ROLE */

        [Required]
        public string Role { get; set; } = "Customer";

        /*
            POSSIBLE ROLES:
            - Admin
            - Staff
            - Customer
        */

        /* PHONE */

        [Required]
        [Phone]
        public string Phone { get; set; }

        /* VEHICLE NUMBER */

        [Required]
        [Display(Name = "Vehicle Number")]
        public string VehicleNumber { get; set; }

        /* ADDRESS */

        public string? Address { get; set; }
    }
}