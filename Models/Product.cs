

using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, ErrorMessage = "Product name cannot be longer than 100 characters")]
        public string ProductName { get; set; }

        [StringLength(200, ErrorMessage = "Description cannot be longer than 200 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Supplier is required")]
        public string Supplier { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a positive number")]
        public int Quantity { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than zero")]
        public decimal UnitPrice { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Invalid date format")]
        public DateTime RestockDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ExpiryDate { get; set; } 
    
        public decimal TotalValue => Quantity * UnitPrice;
    }
}
