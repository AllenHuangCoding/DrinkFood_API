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
            order.O_drink_time = Data.DrinkTime;
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
                   join account in _readDBContext.Account on order.O_create_account_id equals account.A_id
                   join office in _readDBContext.Office on order.O_office_id equals office.O_id
                   where order.O_status != "99" && store.S_status != "99" && account.A_status != "99" && office.O_status != "99"
                   select new ViewOrder
                   {
                       OrderID = order.O_id,
                       OfficeID = order.O_office_id,
                       OfficeName = office.O_name,
                       StoreID = order.O_store_id,
                       StoreName = store.S_name,
                       CreateAccountID = order.O_create_account_id,
                       CreateName = account.A_name,
                       OrderNo = order.O_no,
                       OrderStatus = order.O_status,
                       OrderStatusDesc = "尚未設定",
                       OrderShareUrl = order.O_share_url,
                       CloseTime = order.O_close_time.ToString("yyyy-MM-dd HH:mm"),
                       DrinkTime = order.O_drink_time.ToString("yyyy-MM-dd HH:mm"),
                       CreateTime = order.O_create.ToString("yyyy-MM-dd HH:mm"),
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
