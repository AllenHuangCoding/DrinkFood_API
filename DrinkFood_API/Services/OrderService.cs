using DataBase.Entities;
using DrinkFood_API.Exceptions;
using DrinkFood_API.Models;
using DrinkFood_API.Repository;
using DrinkFood_API.Service;

namespace DrinkFood_API.Services
{
    public class OrderService : BaseService
    {
        [Inject] private readonly OrderRepository _orderRepository;

        [Inject] private readonly OrderDetailRepository _orderDetailRepository;

        public OrderService(IServiceProvider provider) : base()
        {
            provider.Inject(this);
        }

        #region 訂單

        public List<ViewOrder> GetMyOrderList(Guid AccountID, RequestGetMyOrderListModel Request)
        {
            return _orderRepository.GetViewOrder().Where(x => 
                x.OwnerID == AccountID
            ).OrderByDescending(x => x.OrderID).ToList();
        }

        public List<OrderListModel> GetOrderList()
        {
            return _orderRepository.GetViewOrder().OrderBy(x => x.CloseTime).Select(x => new OrderListModel(x)).ToList();
        }

        public ViewOrderAndDetail? GetOrder(Guid OrderID)
        {
            var order = _orderRepository.GetViewOrder().Where(x => x.OrderID == OrderID).FirstOrDefault() ?? throw new ApiException("訂單ID不存在", 400);

            var groupOrderDetail = GetOrderDetailList(OrderID);

            return new ViewOrderAndDetail(order, groupOrderDetail);
        }

        public void PostOrder(RequestPostOrderModel Request)
        {
            _orderRepository.Create(new Order
            {
                O_office_id = Request.OfficeID,
                O_create_account_id = Request.CreateAccountID,
                O_store_id = Request.StoreID,
                O_no = CreateOrderNo(),
                O_type = Request.OrderTypeID,
                O_close_time = Request.CloseTime,
                O_arrival_time = Request.DrinkTime,
            });
        }

        /// <summary>
        /// 更改喝飲料時間 or 結單時間 (限團長與本人)
        /// </summary>
        public void PutOrderTime(Guid OrderID, RequestPutOrderTimeModel Request)
        {
            _orderRepository.PutOrderTime(new PutOrderTimeModel
            {
                OrderID = OrderID,
                DrinkTime = Request.DrinkTime,
                CloseTime = Request.CloseTime,
            });
        }

        /// <summary>
        /// 關閉訂單 (限團長與本人)
        /// </summary>
        public void CloseOrder(Guid OrderID)
        {
            _orderRepository.CloseOrder(OrderID);
        }

        private static string CreateOrderNo()
        {
            return string.Format("O{0}{1:0000}", DateTime.Now.ToString("yyyyMMdd"), new Random().Next(1, 9999));
        }

        #endregion

        #region 訂單詳細

        public List<ViewDetailHistory> GetOrderDetailHistory(Guid AccountID)
        {
            var orderDetail = _orderDetailRepository.GetViewOrderDetail().Where(x =>
                x.AccountID == AccountID
            ).ToList();

            var orderIDs = orderDetail.Select(x => x.OrderID).ToList();

            var order = _orderRepository.GetViewOrder().Where(x => orderIDs.Contains(x.OrderID)).ToList();

            return _orderDetailRepository.CombineDetailHistory(orderDetail, order);
        }

        /// <summary>
        /// 查看訂單內容 (目前皆可以看到，也可以考慮像Nidin自己看自己、團長看全部)
        /// </summary>
        public List<GroupOrderDetailModel> GetOrderDetailList(Guid OrderID)
        {
            var orderDetail = _orderDetailRepository.GetViewOrderDetail().Where(x =>
                x.OrderID == OrderID
            ).ToList();

            return _orderDetailRepository.GroupOrderDetailByName(orderDetail);
        }

        public void PostOrderDetail(RequestPostOrderDetailModel Request)
        {
            _orderDetailRepository.PostOrderDetail(new PostOrderDetailModel
            {
                OD_order_id = Request.OD_order_id,
                OD_drink_food_id = Request.OD_drink_food_id,
                OD_ice_id = Request.OD_ice_id,
                OD_sugar_id = Request.OD_sugar_id,
                OD_size_id = Request.OD_size_id,
                OD_account_id = Request.OD_account_id,
            });
        }

        /// <summary>
        /// 刪除訂單品項 (限團長與本人)
        /// </summary>
        public void DeleteOrderDetail(Guid OrderDetailID)
        {
            // 從Token中解析出AccountID才有辦法接續判斷
            //_orderDetailRepository.CheckMyOrderDetail();
            //_orderDetailRepository.CheckOwnerOrder();

            _orderDetailRepository.DeleteOrderDetail(OrderDetailID);
        }

        /// <summary>
        /// 更改付款方式 (限團長)
        /// </summary>
        public void PutPayment(Guid OrderDetailID, RequestPutPaymentModel Request)
        {
            _orderDetailRepository.PutPayment(OrderDetailID, Request.PaymentID);
        }

        /// <summary>
        /// 更改付款時間 (限團長)
        /// </summary>
        public void PutPaymentDateTime(Guid OrderDetailID, RequestPutPaymentDateTimeModel Request)
        {
            _orderDetailRepository.PutPaymentDatetime(OrderDetailID, Request.PaymentDateTime);
        }

        #endregion
    }
}
