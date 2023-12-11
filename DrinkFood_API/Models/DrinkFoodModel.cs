using DataBase.View;

namespace DrinkFood_API.Models
{
    public class RequestGetDrinkFoodListModel
    {

    }

    public class GroupDrinkFoodModel
    {
        public Guid DrinkFoodTypeID { get; set; }

        public string DrinkFoodTypeDesc { get; set; } = null!;

        public List<ViewDrinkFood> DrinkFoodList { get; set; } = new();
    }
}
