using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Media;

namespace StoreManagement.Models
{
    public class Customers
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public decimal DebtAmount { get; set; }

        #region Not mapped

        [NotMapped]
        public bool IsSelected { get; set; }

        [NotMapped]
        public int IdentityNumber { get; set; }

        [NotMapped]
        public string Character { get; set; }

        [NotMapped]
        public Brush BgColor { get; set; }

        [NotMapped]
        public string DebtAmountString { get; set; }

        #endregion
    }
}
