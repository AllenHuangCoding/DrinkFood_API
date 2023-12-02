using CodeShare.Libs.BaseProject;
using DrinkFood_API.Repository;
using DrinkFood_API.Utility;

namespace DrinkFood_API.Services
{
    public class BrandService : BaseService
    {
        [Inject] private readonly StoreRepository _storeRepository;
        [Inject] private readonly BrandRepository _brandRepository;

        public BrandService(IServiceProvider provider) : base(provider)
        {
            provider.Inject(this);
        }

        public List<OptionsModel> BrandOptions()
        {
            return _brandRepository.GetBrandOption();
        }
    }
}
