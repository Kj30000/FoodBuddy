using SQLite;
using System;

namespace FoodBuddy.Models
{
    public class Stock
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public int ProductId { get; set; } // Foreign key to the Product class

        public int Quantity { get; set; }

        public DateTime PurchaseDate { get; set; }

        // Navigation property to link Stock with Product
        [Ignore]
        public Product? Product { get; set; }


    }
}
