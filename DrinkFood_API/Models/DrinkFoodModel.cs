namespace DrinkFood_API.Models
{
    public class RequestGetDrinkFoodListModel
    {

    }

    public class ViewDrinkFoodModel
    {
        public Guid DrinkFoodID { get; set; }

        public Guid MenuID { get; set; }

        public string DrinkFoodName { get; set; } = null!;

        public int DrinkFoodTypeOrder { get; set; } 

        public string DrinkFoodTypeDesc { get; set; } = null!;

        public int DrinkFoodPrice { get; set; }

        public string? DrinkFoodRemark { get; set; }
    }

    public class GroupDrinkFoodModel
    {
        public string DrinkFoodTypeDesc { get; set; } = null!;

        public List<ViewDrinkFoodModel> DrinkFoodList { get; set; } = new();
    }
}
