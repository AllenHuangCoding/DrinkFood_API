using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataBase.View
{
    public class ViewDrinkFood
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid DrinkFoodID { get; set; }

        public Guid MenuID { get; set; }

        public string DrinkFoodName { get; set; } = null!;

        public int DrinkFoodTypeOrder { get; set; }

        public Guid DrinkFoodTypeID { get; set; }

        public string DrinkFoodTypeDesc { get; set; } = null!;

        public int DrinkFoodPrice { get; set; }

        public string? DrinkFoodRemark { get; set; }
    }
}
