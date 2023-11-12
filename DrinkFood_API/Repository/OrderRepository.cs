using CodeShare.Libs.GenericEntityFramework;
using DataBase.Entities;
using DataBase;
using DrinkFood_API.Models;
using DrinkFood_API.Exceptions;

namespace DrinkFood_API.Repository
{
    public class OrderRepository : BaseTable<EFContext, Order>
    {
        /// <summary>
        /// 建構元 
        /// </summary>
        public OrderRepository(IServiceProvider provider) : base(provider)
        {
            
        }

        public void PutOrderTime(PutOrderTimeModel Data)
        {
            var order = GetById(Data.OrderID) ?? throw new ApiException("訂單ID不存在", 400);
            order.O_arrival_time = Data.DrinkTime;
            order.O_close_time = Data.CloseTime;
            order.O_update = DateTime.Now;
            Update(Data.OrderID, order);
        }

        public void CloseOrder(Guid OrderID)
        {
            var order = GetById(OrderID) ?? throw new ApiException("訂單ID不存在", 400);
            order.O_status = "98";
            order.O_update = DateTime.Now;
            Update(OrderID, order);
        }

        public IQueryable<ViewOrder> GetViewOrder()
        {
            return from order in _readDBContext.Order
                   join store in _readDBContext.Store on order.O_store_id equals store.S_id
                   join brand in _readDBContext.Brand on store.S_brand_id equals brand.B_id
                   join account in _readDBContext.Account on order.O_create_account_id equals account.A_id
                   join office in _readDBContext.Office on order.O_office_id equals office.O_id
                   where order.O_status != "99" && store.S_status != "99" && account.A_status != "99" && office.O_status != "99"
                   select new ViewOrder
                   {
                       OrderID = order.O_id,
                       ArrivalTime = order.O_arrival_time,
                       OfficeID = order.O_office_id,
                       OfficeName = office.O_name,
                       BrandID = brand.B_id,
                       BrandName = brand.B_name,
                       StoreID = order.O_store_id,
                       StoreName = store.S_name,
                       OwnerID = order.O_create_account_id,
                       OwnerName = account.A_name,
                       OrderNo = order.O_no,
                       OrderStatus = order.O_status,
                       OrderStatusDesc = "尚未設定",
                       OrderShareUrl = order.O_share_url,
                       OpenTime = order.O_open_time,
                       CloseTime = order.O_close_time,
                       CloseRemindTime = order.O_close_remind_time,
                       DrinkTime = order.O_arrival_time,
                       Remark = order.O_remark,
                       CreateTime = order.O_create,
                   };
        }

        public Order? Exist(Guid OrderID)
        {
            var order = FindOne(x =>
                x.O_id == OrderID && x.O_status != "99"
            );

            if (order == null)
            {
                return null;
            }
            return order;
        }
    }
}
