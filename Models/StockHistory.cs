using System;

namespace InventoryManagementSystem.Models
{
    public class StockHistory
    {
        public int StockHistoryId { get; set; } // Primary key
        public int ProductId { get; set; } // Foreign key
        public int QuantityChanged { get; set; } // Positive for addition, negative for subtraction
        public DateTime ChangeDate { get; set; } // Date of change
        public string Note { get; set; } = string.Empty; // Optional note about the change

        // Navigation property
        public Product Product { get; set; }
    }
}
