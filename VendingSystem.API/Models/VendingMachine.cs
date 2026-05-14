using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendingSystem.API.Models
{
    [Table("vending_machines")]
    public class VendingMachine
    {
        [Key]
        [Column("machine_id")]
        public int MachineId { get; set; }

        [Required]
        [Column("location")]
        public string Location { get; set; } = string.Empty;

        [Required]
        [Column("model")]
        public string Model { get; set; } = string.Empty;

        [Column("payment_type")]
        public string? PaymentType { get; set; } // "с оплатой картой", "с оплатой наличными", "два вида оплаты"

        [Column("full_income")]
        public decimal FullIncome { get; set; } = 0;

        [Required]
        [Column("serial_number")]
        public string SerialNumber { get; set; } = string.Empty;

        [Required]
        [Column("inventory_number")]
        public string InventoryNumber { get; set; } = string.Empty;

        [Column("manufacturer")]
        public string? Manufacturer { get; set; }

        [Column("manufacture_date")]
        public DateTime ManufactureDate { get; set; }

        [Column("date_of_commissioning")]
        public DateTime DateOfCommissioning { get; set; }

        [Column("last_verification_date")]
        public DateTime? LastVerificationDate { get; set; }

        [Column("verification_interval")]
        public int? VerificationInterval { get; set; } // в месяцах

        [Column("resource_hours")]
        public int ResourceHours { get; set; }

        [Column("date_of_next_fixing")]
        public DateTime? DateOfNextFixing { get; set; }

        [Column("maintenance_time_hours")]
        public int MaintenanceTimeHours { get; set; }

        [Column("machine_status")]
        public string MachineStatus { get; set; } = "Работает"; // "Работает", "Вышел из строя", "В ремонте/на обслуживании"

        [Column("country")]
        public string? Country { get; set; }

        [Column("inventory_date")]
        public DateTime? InventoryDate { get; set; }

        // В Excel это ФИО строкой. Для простоты импорта оставляем строкой.
        [Column("last_checked_by_user")]
        public string? LastCheckedByUser { get; set; }
    }
}