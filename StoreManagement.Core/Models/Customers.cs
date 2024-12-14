using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManagement.Core
{
    public class Customers
    {
        [Key]
        [Column(TypeName = "INTEGER")]
        public int Id { get; set; }

        [Column(TypeName = "TEXT")]
        public string FullName { get; set; }

        [Column(TypeName = "TEXT")]
        public string? Email { get; set; }

        [Column(TypeName = "TEXT")]
        public string? Phone { get; set; }

        [Column(TypeName = "TEXT")]
        public string? Address { get; set; }

        [Column(TypeName = "REAL")]
        [DefaultValue(0)]
        public decimal DebtAmount { get; set; }

        #region Not mapped

        [NotMapped]
        public bool IsSelected { get; set; }

        [NotMapped]
        public int IdentityNumber { get; set; }

        [NotMapped]
        public string Character { get; set; }

        [NotMapped]
        public string BgColor { get; set; }

        [NotMapped]
        public string DebtAmountString { get; set; }

        #endregion
    }
}
