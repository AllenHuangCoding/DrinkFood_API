using Aspose.Cells;
using DataBase.Entities;
using DrinkFood_API.Exceptions;
using DrinkFood_API.Models;
using DrinkFood_API.Repository;
using DrinkFood_API.Service;
using DrinkFood_API.Utility;

namespace DrinkFood_API.Services
{
    public class OrderService : BaseService
    {
        [Inject] private readonly OrderRepository _orderRepository;

        [Inject] private readonly OrderDetailRepository _orderDetailRepository;

        [Inject] private readonly OfficeRepository _officeRepository;

        [Inject] private readonly CodeTableRepository _codeTableRepository;

        [Inject] private readonly StoreRepository _storeRepository;

        public OrderService(IServiceProvider provider) : base()
        {
            provider.Inject(this);
        }

        #region 訂單

        public List<ViewOrder> GetMyOrderList(Guid AccountID, RequestGetMyOrderListModel RequestData)
        {
            return _orderRepository.GetViewOrder().Where(x => 
                x.OwnerID == AccountID
            ).OrderByDescending(x => x.OrderID).ToList();
        }

        public List<OrderListModel> GetOrderList()
        {
            return _orderRepository.GetViewOrder().OrderBy(x => x.CloseTime).Select(x => new OrderListModel(x)).ToList();
        }

        public ViewOrderAndDetail GetOrder(Guid OrderID)
        {
            var order = _orderRepository.GetViewOrder().Where(x => x.OrderID == OrderID).FirstOrDefault() ?? throw new ApiException("訂單ID不存在", 400);

            var groupOrderDetail = GetOrderDetailList(OrderID);

            return new ViewOrderAndDetail(order, groupOrderDetail);
        }

        public ResponseOrderDialogOptions GetCreateOrderDialogOptions(Guid? TypeID)
        {
            return new ResponseOrderDialogOptions
            {
                Office = _officeRepository.GetAll().Select(x => new OptionsModel(x)).ToList(),
                Type = _codeTableRepository.FindAll(x => x.CT_type == "OrderType").Select(x => new OptionsModel(x)).ToList(),
                Store = _storeRepository.GetViewStore().Select(x => new ResponseStoreListModel(x)).Select(x => new OptionsModel(x)).ToList()
            };
        }

        public void PostOrder(RequestPostOrderModel RequestData)
        {
            _orderRepository.Create(new Order
            {
                O_office_id = RequestData.OfficeID,
                O_create_account_id = RequestData.CreateAccountID,
                O_store_id = RequestData.StoreID,
                O_no = CreateOrderNo(),
                O_type = RequestData.TypeID,
                O_arrival_time = RequestData.ArrivalTime,
                O_open_time = RequestData.OpenTime,
                O_close_time = RequestData.CloseTime,
                O_is_public = RequestData.IsPublic,
            });

            // Line Notify / Message 開團通知

            // (HangFire) 設定Line Notify / Message結單前提醒
        }

        /// <summary>
        /// 更改喝飲料時間 or 結單時間 (限團長與本人)
        /// </summary>
        public void PutOrderTime(Guid OrderID, RequestPutOrderTimeModel RequestData)
        {
            _orderRepository.PutOrderTime(new PutOrderTimeModel
            {
                OrderID = OrderID,
                DrinkTime = RequestData.DrinkTime,
                CloseTime = RequestData.CloseTime,
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

        public void PostOrderDetail(RequestPostOrderDetailModel RequestData)
        {
            _orderDetailRepository.PostOrderDetail(new PostOrderDetailModel
            {
                OD_order_id = RequestData.OD_order_id,
                OD_drink_food_id = RequestData.OD_drink_food_id,
                OD_ice_id = RequestData.OD_ice_id,
                OD_sugar_id = RequestData.OD_sugar_id,
                OD_size_id = RequestData.OD_size_id,
                OD_account_id = RequestData.OD_account_id,
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
        public void PutPayment(Guid OrderDetailID, RequestPutPaymentModel RequestData)
        {
            _orderDetailRepository.PutPayment(OrderDetailID, RequestData.PaymentID);
        }

        /// <summary>
        /// 更改付款時間 (限團長)
        /// </summary>
        public void PutPaymentDateTime(Guid OrderDetailID, RequestPutPaymentDateTimeModel RequestData)
        {
            _orderDetailRepository.PutPaymentDatetime(OrderDetailID, RequestData.PaymentDateTime);
        }

        #endregion
    }
}
