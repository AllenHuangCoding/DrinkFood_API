using DrinkFood_API.Exceptions;
using DrinkFood_API.Models;
using DrinkFood_API.Repository;
using DrinkFood_API.Service;

namespace DrinkFood_API.Services
{
    public class MenuService : BaseService
    {
        [Inject] private readonly MenuRepository _menuRepository;

        [Inject] private readonly StoreRepository _storeRepository;

        public MenuService(IServiceProvider provider) : base()
        {
            provider.Inject(this);
        }

        public List<ViewMenu> GetBrandMenuList(Guid BrandID)
        {
            return _menuRepository.GetBrandMenuList(BrandID);
        }

        public ViewMenu? GetStoreMenu(Guid StoreID)
        {
            return _menuRepository.GetStoreMenu(StoreID);
        }
    }
}
