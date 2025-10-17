using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartGearOnline.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, 1000000, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, 1000000, ErrorMessage = "Shipping Cost can not be 0")]
        public int ShippingCost { get; set; }

        // Debited Amount(Price + Shipping)
        public decimal TotalPrice => CalculateTotalPrice();

        private decimal CalculateTotalPrice()
        {
            return Math.Round(Price + ShippingCost, 2);
        }
    }
}