using DataBase.Entities;
using CodeShare.Libs.BaseProject;
using DrinkFood_API.Models;
using DrinkFood_API.Repository;
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

        [Inject] private readonly AuthService _authService;

        public OrderService(IServiceProvider provider) : base(provider)
        {
            provider.Inject(this);
        }

        #region 訂單查詢 (清單/詳細/選項選單)

        /// <summary>
        /// 訂單清單
        /// </summary>
        /// <returns></returns>
        public List<OrderListModel> GetOrderList()
        {
            // 開團清單 = 公團 + 私團
            return _orderRepository.GetViewOrder().OrderBy(x => x.CloseTime).Select(x => new OrderListModel(x).SetButton(_authService.UserID)).ToList();
        }

        /// <summary>
        /// 訂單詳細
        /// </summary>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        /// <exception cref="ApiException"></exception>
        public ViewOrderAndDetail GetOrder(Guid OrderID)
        {
            var order = _orderRepository.GetViewOrder().Where(x => x.OrderID == OrderID).FirstOrDefault() ?? throw new ApiException("訂單ID不存在", 400);

            var groupOrderDetail = GetOrderDetailList(OrderID);

            return new ViewOrderAndDetail(order, groupOrderDetail, _authService.UserID);
        }

        /// <summary>
        /// 訂單選項選單
        /// </summary>
        /// <param name="TypeID"></param>
        /// <returns></returns>
        public ResponseOrderDialogOptions GetCreateOrderDialogOptions(Guid? TypeID)
        {
            return new ResponseOrderDialogOptions
            {
                Office = _officeRepository.GetAll().Select(x => new OptionsModel(x)).ToList(),
                Type = _codeTableRepository.FindAll(x => x.CT_type == "OrderType").OrderBy(x => x.CT_order).Select(x => new OptionsModel(x)).ToList(),
                Store = _storeRepository.GetViewStore().Select(x => new ResponseStoreListModel(x)).Select(x => new OptionsModel(x)).ToList()
            };
        }

        #endregion

        #region 訂單流程動作 (新增訂單/加入訂單)

        /// <summary>
        /// 新增訂單
        /// </summary>
        /// <param name="RequestData"></param>
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

            // 公團的 Line Notify / Email 開團通知

            // (HangFire) 設定Line Notify / Message結單前提醒
        }

        /// <summary>
        /// 加入訂單
        /// </summary>
        /// <param name="OrderID"></param>
        public void JoinOrder(Guid OrderID)
        {
            _orderDetailRepository.Create(new OrderDetail
            {
                OD_order_id = OrderID,
                OD_account_id = _authService.UserID,
                OD_create_account_id = _authService.UserID,
            });
        }

        #endregion

        #region 更改訂單狀態 (關閉訂單/完成訂單)

        /// <summary>
        /// 關閉訂單 (限團長與本人)
        /// </summary>
        public void CloseOrder(Guid OrderID)
        {
            _orderRepository.CloseOrder(OrderID);
        }

        /// <summary>
        /// 完成訂單 (限團長)
        /// </summary>
        /// <param name="OrderID"></param>
        public void FinishOrder(Guid OrderID)
        {
            _orderRepository.FinishOrder(OrderID);
        }

        #endregion

        #region 更改訂單欄位 (用餐時間/結單時間)

        /// <summary>
        /// 更改用餐時間 (限團長)
        /// </summary>
        public void PutArrivalTime(Guid OrderID, RequestPutArrivalTimeModel RequestData)
        {
            _orderRepository.PutArrivalTime(OrderID, RequestData.ArrivalTime);
        }

        /// <summary>
        /// 更改結單時間 (限團長)
        /// </summary>
        public void PutCloseTime(Guid OrderID, RequestPutCloseTimeModel RequestData)
        {
            _orderRepository.PutCloseTime(OrderID, RequestData.CloseTime);
        }

        #endregion

        #region 訂單通知 (遲到/遲到抵達)

        /// <summary>
        /// 遲到通知
        /// </summary>
        /// <param name="OrderID"></param>
        public void DelayNotify(Guid OrderID)
        {

        }

        /// <summary>
        /// 遲到抵達通知
        /// </summary>
        /// <param name="OrderID"></param>
        public void DelayArrivalNotify(Guid OrderID)
        {

        }


        #endregion

        #region 訂單明細查詢 (訂購者明細/歷史紀錄)

        /// <summary>
        /// 訂單明細 (依訂購者分群)
        /// </summary>
        public List<GroupOrderDetailModel> GetOrderDetailList(Guid OrderID)
        {
            var orderDetail = _orderDetailRepository.GetViewOrderDetail().Where(x =>
                x.OrderID == OrderID
            ).ToList();

            return _orderDetailRepository.GroupOrderDetailByName(orderDetail, _authService.UserID);
        }

        /// <summary>
        /// 訂單明細歷史紀錄
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        public List<ViewDetailHistory> GetOrderDetailHistory(Guid AccountID)
        {
            var orderDetail = _orderDetailRepository.GetViewOrderDetail().Where(x =>
                x.DetailAccountID == AccountID
            ).ToList();

            var orderIDs = orderDetail.Select(x => x.OrderID).ToList();

            var order = _orderRepository.GetViewOrder().Where(x => orderIDs.Contains(x.OrderID)).ToList();

            return _orderDetailRepository.CombineDetailHistory(orderDetail, order, _authService.UserID);
        }

        #endregion

        #region 訂單明細流程動作 (加入品項/刪除品項)

        /// <summary>
        /// 加入品項 (限本人)
        /// </summary>
        /// <param name="RequestData"></param>
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
        /// 刪除品項 (限團長與本人)
        /// </summary>
        public void DeleteOrderDetail(Guid OrderDetailID)
        {
            // 從Token中解析出AccountID才有辦法接續判斷
            //_orderDetailRepository.CheckMyOrderDetail();
            //_orderDetailRepository.CheckOwnerOrder();

            _orderDetailRepository.DeleteOrderDetail(OrderDetailID);
        }

        #endregion

        #region 更改訂單明細欄位 (更改付款方式/更改付款時間/更改取餐狀態)

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

        /// <summary>
        /// 更改取餐狀態 (限團長)
        /// </summary>
        /// <param name="OrderDetailID"></param>
        public void PutPickup(Guid OrderDetailID)
        {
            
        }

        #endregion

        #region 私有方法 (產生訂單編號)

        private static string CreateOrderNo()
        {
            return string.Format("O{0}{1:0000}", DateTime.Now.ToString("yyyyMMdd"), new Random().Next(1, 9999));
        }

        #endregion
    }
}
