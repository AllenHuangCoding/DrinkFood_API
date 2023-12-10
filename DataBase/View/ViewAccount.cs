using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataBase.View
{
    public class ViewAccount
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AccountID { get; set; }

        public string Name { get; set; } = null!;

        public string? Brief { get; set; }

        public string Email { get; set; } = null!;

        public string? LineID { get; set; }

        public bool LineNotify { get; set; }

        public bool LunchNotify { get; set; }

        public bool DrinkNotify { get; set; }

        public int CloseNotify { get; set; }

        public Guid? DefaultLunchPayment { get; set; }

        public string? DefaultLunchPaymentDesc { get; set; }

        public Guid? DefaultDrinkPayment { get; set; }

        public string? DefaultDrinkPaymentDesc { get; set; }

        public bool IsAdmin { get; set; }
    }
}
