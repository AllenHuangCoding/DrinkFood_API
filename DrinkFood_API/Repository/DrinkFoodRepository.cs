using CodeShare.Libs.GenericEntityFramework;
using DataBase.Entities;
using DataBase;
using DrinkFood_API.Models;

namespace DrinkFood_API.Repository
{
    public class DrinkFoodRepository : BaseTable<EFContext, DrinkFood>
    {
        [Inject] private readonly MenuRepository _menuRepository;

        /// <summary>
        /// 建構元
        /// </summary>
        public DrinkFoodRepository(IServiceProvider provider) : base(provider)
        {
            provider.Inject(this);
        }

        public List<ViewDrinkFoodModel> GetDrinkFoodList(Guid StoreID)
        {
            var menu = _menuRepository.GetStoreMenu(StoreID)!;
            return GetViewDrinkFood().Where(x => 
                x.MenuID == menu.MenuID
            ).OrderBy(x => x.DrinkFoodTypeOrder).ThenBy(x => x.DrinkFoodName).ToList();
        }

        public List<GroupDrinkFoodModel> GroupDrinkFoodByType(List<ViewDrinkFoodModel> Data)
        {
            return Data.GroupBy(x =>
                x.DrinkFoodTypeDesc
            ).Select(x =>
                new GroupDrinkFoodModel
                {
                    DrinkFoodTypeDesc = x.Key,
                    DrinkFoodList = x.ToList()
                }
            ).ToList();
        }

        public IQueryable<ViewDrinkFoodModel> GetViewDrinkFood()
        {
            return from drinkFood in _readDBContext.DrinkFood
                   join codeTable in _readDBContext.CodeTable.Where(x => x.CT_type == "DrinkFoodType") on drinkFood.DF_type equals codeTable.CT_id
                   where drinkFood.DF_status != "99"
                   select new ViewDrinkFoodModel
                   {
                       DrinkFoodID = drinkFood.DF_id,
                       MenuID = drinkFood.DF_menu_id,
                       DrinkFoodName = drinkFood.DF_name,
                       DrinkFoodTypeOrder = codeTable.CT_order,
                       DrinkFoodTypeDesc = codeTable.CT_desc,
                       DrinkFoodPrice = drinkFood.DF_price,
                       DrinkFoodRemark = drinkFood.DF_remark ?? "",
                   };
        }
    }
}
