using DrinkFood_API.Repository;
using DrinkFood_API.Service;
using DrinkFood_API.Utility;

namespace DrinkFood_API.Services
{
    public class BrandService : BaseService
    {
        [Inject] private readonly StoreRepository _storeRepository;
        [Inject] private readonly BrandRepository _brandRepository;

        public BrandService(IServiceProvider provider) : base()
        {
            provider.Inject(this);
        }

        public List<OptionsModel> BrandOptions()
        {
            return _brandRepository.GetBrandOption();
        }
    }
}
