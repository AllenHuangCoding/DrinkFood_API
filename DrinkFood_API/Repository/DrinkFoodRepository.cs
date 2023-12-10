using CodeShare.Libs.GenericEntityFramework;
using DataBase.Entities;
using DataBase;
using DrinkFood_API.Models;
using DataBase.View;

namespace DrinkFood_API.Repository
{
    public class ViewDrinkFoodRepository : BaseView<EFContext, ViewDrinkFood> 
    { 
        public ViewDrinkFoodRepository(IServiceProvider provider) : base(provider)
        {
            provider.Inject(this);
        }
    }

    public class DrinkFoodRepository : BaseTable<EFContext, DrinkFood>
    {
        [Inject] private readonly MenuRepository _menuRepository;

        [Inject] private readonly ViewDrinkFoodRepository _viewDrinkFoodRepository;

        /// <summary>
        /// 建構元
        /// </summary>
        public DrinkFoodRepository(IServiceProvider provider) : base(provider)
        {
            provider.Inject(this);
        }

        public List<ViewDrinkFood> GetDrinkFoodList(Guid StoreID)
        {
            var menu = _menuRepository.GetStoreMenu(StoreID)!;
            return _viewDrinkFoodRepository.FindAll(x => 
                x.MenuID == menu.MenuID
            ).OrderBy(x => x.DrinkFoodTypeOrder).ThenBy(x => x.DrinkFoodName).ToList();
        }

        public List<GroupDrinkFoodModel> GroupDrinkFoodByType(List<ViewDrinkFood> Data)
        {
            return Data.GroupBy(x => new {
                x.DrinkFoodTypeID,
                x.DrinkFoodTypeDesc
            }).Select(x =>
                new GroupDrinkFoodModel
                {
                    DrinkFoodTypeID = x.Key.DrinkFoodTypeID,
                    DrinkFoodTypeDesc = x.Key.DrinkFoodTypeDesc,
                    DrinkFoodList = x.OrderBy(x => x.DrinkFoodTypeOrder).ToList()
                }
            ).ToList();
        }
    }
}
