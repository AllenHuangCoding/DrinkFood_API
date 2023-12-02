using CodeShare.Libs.GenericEntityFramework;
using DataBase;
using DataBase.Entities;
using CodeShare.Libs.BaseProject;
using DrinkFood_API.Models;

namespace DrinkFood_API.Repository
{
    public class StoreRepository : BaseTable<EFContext, Store>
    {
        /// <summary>
        /// 建構元
        /// </summary>
        public StoreRepository(IServiceProvider provider) : base(provider)
        {

        }

        public ResponseStoreListModel GetStore(Guid StoreID)
        {
            var store = GetViewStore().Where(x =>
                x.S_id == StoreID
            ).FirstOrDefault() ?? throw new ApiException("店家資料不存在", 400);

            return new ResponseStoreListModel(store);
        }

        public List<ResponseStoreListModel> GetStoreList(RequestStoreListModel RequestData)
        {
            return GetViewStore().Select(x => new ResponseStoreListModel(x)).ToList();
        }

        public IQueryable<ViewStore> GetViewStore()
        {
            return from store in _readDBContext.Store
                   join brand in _readDBContext.Brand on store.S_brand_id equals brand.B_id
                   join codeTable in _readDBContext.CodeTable.Where(x => x.CT_type == "BrandType") on brand.B_type equals codeTable.CT_id
                   where store.S_status != "99" && brand.B_status != "99"
                   select new ViewStore
                   {
                       B_id = brand.B_id,
                       B_name = brand.B_name,
                       B_logo = brand.B_logo,
                       B_line_id = brand.B_line_id,
                       B_official_url = brand.B_official_url,
                       B_type_desc = codeTable.CT_desc,
                       S_id = store.S_id,
                       S_brand_id = store.S_brand_id,
                       S_name = store.S_name,
                       S_address = store.S_address,
                       S_line_id= store.S_line_id,
                       S_menu_area_id = store.S_menu_area_id,
                       S_phone = store.S_phone,
                       S_remark = store.S_remark,
                   };
        }
    }
}
