using System.ComponentModel.DataAnnotations;

namespace VehiclePartsManagementSystem.Models
{
    public class Sale
    {
        public int Id { get; set; }

        /* CUSTOMER */

        [Required]
        public int CustomerId { get; set; }

        public Customer? Customer { get; set; }

        /* PART */

        [Required]
        public int PartId { get; set; }

        public Part? Part { get; set; }

        /* QUANTITY */

        [Required]
        public int Quantity { get; set; }

        /* ORIGINAL TOTAL */

        [Required]
        public decimal TotalAmount { get; set; }

        /* DISCOUNT */

        public decimal DiscountAmount { get; set; } = 0;

        /* FINAL TOTAL */

        public decimal FinalAmount { get; set; }

        /*
            LOYALTY RULE:
            If TotalAmount > 5000
            => 10% Discount
        */

        /* SALE DATE */

        public DateTime SaleDate { get; set; } =
            DateTime.Now;
    }
}