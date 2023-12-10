using CodeShare.Libs.BaseProject;
using CodeShare.Libs.BaseProject.Extensions;
using DrinkFood_API.Models;
using DrinkFood_API.Repository;

namespace DrinkFood_API.Services
{
    public class StoreService : BaseService
    {
        [Inject] private readonly StoreRepository _storeRepository;

        [Inject] private readonly OrderRepository _orderRepository;

        [Inject] private readonly ViewOrderRepository _viewOrderRepository;

        public StoreService(IServiceProvider provider) : base(provider) 
        {
            provider.Inject(this);
        }

        public ResponseStoreListModel GetStore(Guid StoreID)
        {
            return _storeRepository.GetStore(StoreID);
        }

        public List<ResponseStoreListModel> GetStoreList(RequestStoreListModel RequestData)
        {
            var response = _storeRepository.GetStoreList(RequestData);

            var storeIDs = response.Select(x => x.StoreID).ToList();
            var storeOrder = _viewOrderRepository.GetAll().Where(x => storeIDs.Contains(x.StoreID)).ToList();

            foreach (var item in response)
            {
                var lastOrder = storeOrder.Where(o =>
                    o.StoreID == item.StoreID
                ).OrderByDescending(x =>
                    x.ArrivalTime
                ).FirstOrDefault();

                if (lastOrder != null)
                {
                    item.PreviousOrderDate = lastOrder.ArrivalTime.ToDateHourMinute();
                }
            }

            return response;
        }
    }
}
