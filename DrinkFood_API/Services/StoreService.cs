using DrinkFood_API.Models;
using DrinkFood_API.Repository;
using DrinkFood_API.Service;

namespace DrinkFood_API.Services
{
    public class StoreService : BaseService
    {
        [Inject] private readonly StoreRepository _storeRepository;

        [Inject] private readonly OrderRepository _orderRepository;

        public StoreService(IServiceProvider provider) : base() 
        {
            provider.Inject(this);
        }

        public List<ResponseStoreListModel> GetStoreList(RequestStoreListModel Request)
        {
            var response = _storeRepository.GetStoreList(Request);

            var storeIDs = response.Select(x => x.StoreID).ToList();
            var storeOrder = _orderRepository.GetViewOrder().Where(x => storeIDs.Contains(x.StoreID)).ToList();

            foreach (var item in response)
            {
                var lastOrder = storeOrder.Where(o =>
                    o.StoreID == item.StoreID
                ).OrderByDescending(x =>
                    x.DrinkTime
                ).FirstOrDefault();

                if (lastOrder != null)
                {
                    item.PreviousOrderDate = lastOrder.DrinkTime;
                }
            }

            return response;
        }
    }
}
