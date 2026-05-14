using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendingSystem.API.Models
{
    [Table("products")]
    public class Product
    {
        [Key]
        [Column("product_id")]
        public int ProductId { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("description")]
        public string? Description { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("in_stock")]
        public int InStock { get; set; }

        [Column("min_stock")]
        public int MinStock { get; set; }

        [Column("propensity_to_sell")]
        public decimal? PropensityToSell { get; set; }
    }
}