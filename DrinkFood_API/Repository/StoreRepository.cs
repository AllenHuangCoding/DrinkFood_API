using CodeShare.Libs.GenericEntityFramework;
using DataBase;
using DataBase.Entities;
using CodeShare.Libs.BaseProject;
using DrinkFood_API.Models;
using DataBase.View;

namespace DrinkFood_API.Repository
{
    public class ViewStoreRepository : BaseView<EFContext, ViewStore>
    {
        public ViewStoreRepository(IServiceProvider provider) : base(provider)
        {
            provider.Inject(this);
        }
    }

    public class StoreRepository : BaseTable<EFContext, Store>
    {
        [Inject] private readonly ViewStoreRepository _viewStoreRepository;

        /// <summary>
        /// 建構元
        /// </summary>
        public StoreRepository(IServiceProvider provider) : base(provider)
        {
            provider.Inject(this);
        }

        public ResponseStoreListModel GetStore(Guid StoreID)
        {
            var store = _viewStoreRepository.GetAll().FirstOrDefault(x =>
                x.S_id == StoreID
            ) ?? throw new ApiException("店家資料不存在", 400);

            return new ResponseStoreListModel(store);
        }

        public List<ResponseStoreListModel> GetStoreList(RequestStoreListModel RequestData)
        {
            return _viewStoreRepository.GetAll().Select(x => new ResponseStoreListModel(x)).ToList();
        }
    }
}
