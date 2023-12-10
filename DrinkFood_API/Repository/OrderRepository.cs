using CodeShare.Libs.GenericEntityFramework;
using DataBase.Entities;
using DataBase;
using DrinkFood_API.Models;
using CodeShare.Libs.BaseProject;
using DataBase.View;

namespace DrinkFood_API.Repository
{
    public class ViewOrderRepository : BaseView<EFContext, ViewOrder>
    {
        public ViewOrderRepository(IServiceProvider provider) : base(provider)
        {
            provider.Inject(this);
        }
    }

    public class OrderRepository : BaseTable<EFContext, Order>
    {
        [Inject] private readonly ViewOrderRepository _viewOrderRepository;

        /// <summary>
        /// 建構元 
        /// </summary>
        public OrderRepository(IServiceProvider provider) : base(provider)
        {
            provider.Inject(this);
        }

        #region 訂單查詢 (View)

        public OrderListModel GetOrderListData(Guid OrderID)
        {
            ViewOrder? viewOrder = _viewOrderRepository.GetAll().FirstOrDefault(x =>
                x.OrderID == OrderID
            ) ?? throw new ApiException("訂單ID不存在", 400);

            return new OrderListModel(viewOrder);
        }

        #endregion

        #region 更改訂單欄位 (用餐時間/結單時間)

        /// <summary>
        /// 更改用餐時間
        /// </summary>
        /// <param name="OrderID"></param>
        /// <param name="time"></param>
        /// <exception cref="ApiException"></exception>
        public void PutArrivalTime(Guid OrderID, DateTime time)
        {
            var order = GetById(OrderID) ?? throw new ApiException("訂單ID不存在", 400);
            order.O_arrival_time = time;
            order.O_update = DateTime.Now;
            Update(OrderID, order);
        }

        /// <summary>
        /// 更改結單時間
        /// </summary>
        /// <param name="OrderID"></param>
        /// <param name="time"></param>
        /// <exception cref="ApiException"></exception>
        public void PutCloseTime(Guid OrderID, DateTime time)
        {
            var order = GetById(OrderID) ?? throw new ApiException("訂單ID不存在", 400);
            order.O_close_time = time;
            order.O_update = DateTime.Now;
            Update(OrderID, order);
        }

        #endregion

        #region 更改訂單狀態 (關閉訂單/完成訂單)

        /// <summary>
        /// 關閉訂單 (限團長與本人)
        /// </summary>
        /// <param name="OrderID"></param>
        /// <exception cref="ApiException"></exception>
        public void CloseOrder(Guid OrderID)
        {
            var order = GetById(OrderID) ?? throw new ApiException("訂單ID不存在", 400);
            order.O_status = "98";
            order.O_update = DateTime.Now;
            Update(OrderID, order);
        }

        /// <summary>
        /// 完成訂單 (限團長)
        /// </summary>
        /// <param name="OrderID"></param>
        /// <exception cref="ApiException"></exception>
        public void FinishOrder(Guid OrderID)
        {
            var order = GetById(OrderID) ?? throw new ApiException("訂單ID不存在", 400);
            order.O_status = "02";
            order.O_update = DateTime.Now;
            Update(OrderID, order);
        }

        #endregion

        #region 其他方法 (檢查訂單團長)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AccountID"></param>
        /// <param name="OrderID"></param>
        /// <exception cref="ApiException"></exception>
        public void CheckOwnerOrder(Guid AccountID, Guid OrderID)
        {
            var order = _viewOrderRepository.GetAll().FirstOrDefault(x =>
                x.OrderID == OrderID
            ) ?? throw new ApiException("訂單不存在", 400);

            if (order.OwnerID != AccountID)
            {
                throw new ApiException("非團長權限", 400);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Entity"></param>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        public bool IsOwnerOrder(ViewOrder Entity, Guid AccountID)
        {
            if (Entity.OwnerID == AccountID)
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}
