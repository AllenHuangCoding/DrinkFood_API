using DrinkFood_API.Models;
using DrinkFood_API.Repository;
using DrinkFood_API.Service;

namespace DrinkFood_API.Services
{
    public class StoreService : BaseService
    {
        [Inject] private readonly StoreRepository _storeRepository;

        public StoreService(IServiceProvider provider) : base() 
        {
            provider.Inject(this);
        }

        public List<ResponseStoreListModel> GetStoreList(RequestStoreListModel Request)
        {
            return _storeRepository.GetStoreList(Request);
        }
    }
}
