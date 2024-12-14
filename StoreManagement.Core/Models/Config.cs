using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManagement.Core
{
    public class Config
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Key { get; set; }

        [Column(TypeName = "TEXT")]
        public string? Value { get; set; }

        [Column(TypeName = "TEXT")]
        public string? Description { get; set; }
    }
}
