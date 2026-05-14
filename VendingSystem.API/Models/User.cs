using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendingSystem.API.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("userid")]
        public int UserId { get; set; }

        [Required]
        [Column("fullname")]
        public string FullName { get; set; } = string.Empty;

        // В Excel Contacts содержит Email и Телефон через запятую.
        // Мы добавили отдельную колонку Email для удобства аутентификации.
        [Column("email")]
        public string? Email { get; set; }

        [Column("contacts")]
        public string? Contacts { get; set; }

        [Column("role")]
        public string Role { get; set; } = "Оператор";

        // Поле для пароля (необходимо для аутентификации, хотя его нет в Excel)
        // При импорте мы заполним его хешем от временного пароля "123456"
        [Column("password_hash")]
        public string? PasswordHash { get; set; }
    }
}