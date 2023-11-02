using CodeShare.Libs.GenericEntityFramework;
using DataBase.Entities;
using DataBase;
using DrinkFood_API.Models;
using DrinkFood_API.Exceptions;

namespace DrinkFood_API.Repository
{
    public class MenuRepository : BaseTable<EFContext, Menu>
    {
        [Inject] private readonly CodeTableRepository _codeTableRepository;

        [Inject] private readonly StoreRepository _storeRepository;

        /// <summary>
        /// 建構元
        /// </summary>
        public MenuRepository(IServiceProvider provider) : base(provider)
        {
            provider.Inject(this);
        }

        public List<ViewMenu> GetBrandMenuList(Guid BrandID)
        {
            var brandMenu = GetViewMenu().Where(x =>
                x.BrandID == BrandID
            ).OrderByDescending(x =>
                x.MenuCreate
            ).ToList();

            return brandMenu.GroupBy(x =>
                x.MenuAreaID
            ).SelectMany(x =>
                x.Take(1)
            ).ToList();
        }

        public ViewMenu? GetStoreMenu(Guid StoreID)
        {
            var store = _storeRepository.FindOne(x => x.S_id == StoreID) ?? throw new ApiException("商店ID不存在", 400);

            return GetViewMenu().Where(x =>
                x.BrandID == store.S_brand_id && x.MenuAreaID == store.S_menu_area_id
            ).FirstOrDefault() ?? throw new ApiException("店家菜單不存在", 400);
        }

        public IQueryable<ViewMenu> GetViewMenu()
        {
            return from menu in _readDBContext.Menu
                   join brand in _readDBContext.Brand on menu.M_brand_id equals brand.B_id
                   join areaCodeTable in _readDBContext.CodeTable.Where(x => x.CT_type == "MenuArea") on menu.M_area_id equals areaCodeTable.CT_id
                   where menu.M_status != "99" && brand.B_status != "99"
                   select new ViewMenu
                   {
                       MenuID = menu.M_id,
                       MenuAreaID = menu.M_area_id,
                       MenuAreaDesc = areaCodeTable.CT_desc,
                       MenuCreate = menu.M_create,
                       BrandID = brand.B_id,
                       BrandName = brand.B_name,
                   };
        }
    }
}
