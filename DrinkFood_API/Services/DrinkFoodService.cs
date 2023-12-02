using CodeShare.Libs.BaseProject;
using DrinkFood_API.Models;
using DrinkFood_API.Repository;

namespace DrinkFood_API.Services
{
    public class DrinkFoodService : BaseService
    {
        [Inject] private readonly DrinkFoodRepository _drinkFoodRepository;

        public DrinkFoodService(IServiceProvider provider) : base(provider)
        {
            provider.Inject(this);
        }

        public List<GroupDrinkFoodModel> GetDrinkFoodList(Guid StoreID)
        {
            var drinkFoodList = _drinkFoodRepository.GetDrinkFoodList(StoreID);
            return _drinkFoodRepository.GroupDrinkFoodByType(drinkFoodList);
        }
    }
}
