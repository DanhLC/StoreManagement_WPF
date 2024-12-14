using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManagement.Core
{
    public class Users
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "TEXT")]
        public string Username { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "TEXT")]
        public string Password { get; set; } = string.Empty;

        [Column(TypeName = "TEXT")]
        public string? FullName { get; set; }

        [Column(TypeName = "TEXT")]
        public string? Role { get; set; }

        [Column(TypeName = "TEXT")]
        public string? Email { get; set; }
    }
}
