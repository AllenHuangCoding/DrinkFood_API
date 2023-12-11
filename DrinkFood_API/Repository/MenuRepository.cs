using CodeShare.Libs.GenericEntityFramework;
using DataBase.Entities;
using DataBase;
using DrinkFood_API.Models;
using CodeShare.Libs.BaseProject;
using DataBase.View;

namespace DrinkFood_API.Repository
{
    public class ViewMenuRepository : BaseView<EFContext, ViewMenu>
    {
        public ViewMenuRepository(IServiceProvider provider) : base(provider)
        {
            provider.Inject(this);
        }
    }

    public class MenuRepository : BaseTable<EFContext, Menu>
    {
        [Inject] private readonly CodeTableRepository _codeTableRepository;

        [Inject] private readonly StoreRepository _storeRepository;

        [Inject] private readonly ViewMenuRepository _viewMenuRepository;

        /// <summary>
        /// 建構元
        /// </summary>
        public MenuRepository(IServiceProvider provider) : base(provider)
        {
            provider.Inject(this);
        }

        public List<ViewMenu> GetBrandMenuList(Guid BrandID)
        {
            var brandMenu = _viewMenuRepository.FindAll(x =>
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

        public ViewMenu GetStoreMenu(Guid StoreID)
        {
            var store = _storeRepository.FindOne(x => x.S_id == StoreID) ?? throw new ApiException("商店ID不存在", 400);
            
            var menu = _viewMenuRepository.GetAll().FirstOrDefault(x =>
                x.BrandID == store.S_brand_id && x.MenuAreaID == store.S_menu_area_id
            ) ?? throw new ApiException("店家菜單不存在", 400);

            return menu;
        }
    }
}
