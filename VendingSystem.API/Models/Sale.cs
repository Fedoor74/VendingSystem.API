using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendingSystem.API.Models
{
    [Table("sales")]
    public class Sale
    {
        [Key]
        [Column("sale_id")]
        public int SaleId { get; set; }

        [Column("machine_id")]
        public int? MachineId { get; set; } // FK to VendingMachine

        [ForeignKey("MachineId")]
        public VendingMachine? Machine { get; set; }

        [Column("product_id")]
        public int? ProductId { get; set; } // FK to Product

        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("sale_sum")]
        public decimal SaleSum { get; set; }

        [Column("sale_datetime")]
        public DateTime SaleDateTime { get; set; }

        [Column("payment_type")]
        public string? PaymentType { get; set; } // "Карта", "Наличные", "QR-код"
    }
}