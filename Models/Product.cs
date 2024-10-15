

using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [StringLength(100)]
        public string ProductName { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        public string Supplier { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(0, double.MaxValue)]
        public decimal UnitPrice { get; set; }

        [DataType(DataType.Date)]
        public DateTime RestockDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ExpiryDate { get; set; } // Allow nullable for products without expiry

        // Calculated Property
        public decimal TotalValue => Quantity * UnitPrice;
    }
}
