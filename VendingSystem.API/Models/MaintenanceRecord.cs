using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendingSystem.API.Models
{
    [Table("maintenance_records")]
    public class MaintenanceRecord
    {
        [Key]
        [Column("note_id")]
        public int NoteId { get; set; }

        [Column("machine_id")]
        public int? MachineId { get; set; } // FK to VendingMachine

        [ForeignKey("MachineId")]
        public VendingMachine? Machine { get; set; }

        [Column("maintenance_date")]
        public DateTime MaintenanceDate { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("problems")]
        public string? Problems { get; set; }

        // В Excel колонка называется DoneByUser, но там лежат ID (1, 2, 3...).
        // Создадим связь с таблицей Users.
        [Column("done_by_user")]
        public int? DoneByUserId { get; set; } // FK to User

        [ForeignKey("DoneByUserId")]
        public User? DoneByUserNavigation { get; set; }
    }
}