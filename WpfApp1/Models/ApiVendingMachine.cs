using System;

namespace VendingFranchisee.Desktop.Models
{
    public class ApiVendingMachine
    {
        public int MachineId { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string PaymentType { get; set; } = string.Empty;
        public decimal FullIncome { get; set; }
        public string SerialNumber { get; set; } = string.Empty;
        public string InventoryNumber { get; set; } = string.Empty;
        public string Manufacturer { get; set; } = string.Empty;
        public DateTime ManufactureDate { get; set; }
        public DateTime DateOfCommissioning { get; set; }
        public DateTime? LastVerificationDate { get; set; }
        public int? VerificationInterval { get; set; }
        public int ResourceHours { get; set; }
        public DateTime? DateOfNextFixing { get; set; }
        public int MaintenanceTimeHours { get; set; }
        public string MachineStatus { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public DateTime? InventoryDate { get; set; }
        public string LastCheckedByUser { get; set; } = string.Empty;

        // Для эмуляции
        public int ModemId { get; set; } = 1;
        public string FranchiseeName { get; set; } = "ООО Вендинг";
    }
}