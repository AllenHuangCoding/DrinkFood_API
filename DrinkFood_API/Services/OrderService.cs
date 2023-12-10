using DataBase.Entities;
using CodeShare.Libs.BaseProject;
using DrinkFood_API.Models;
using DrinkFood_API.Repository;
using DrinkFood_API.Utility;
using CodeShare.Libs.BaseProject.Extensions;
using NPOI.OpenXmlFormats.Dml;
using DataBase.View;

namespace DrinkFood_API.Services
{
    public class OrderService : BaseService
    {
        [Inject] private readonly OrderRepository _orderRepository;

        [Inject] private readonly ViewOrderRepository _viewOrderRepository;

        [Inject] private readonly OrderDetailRepository _orderDetailRepository;

        [Inject] private readonly ViewOrderDetailRepository _viewOrderDetailRepository;

        [Inject] private readonly OfficeRepository _officeRepository;

        [Inject] private readonly CodeTableRepository _codeTableRepository;

        [Inject] private readonly StoreRepository _storeRepository;

        [Inject] private readonly ViewStoreRepository _viewStoreRepository;

        [Inject] private readonly IAuthService _authService;

        [Inject] private readonly LineService _lineService;

        public OrderService(IServiceProvider provider) : base(provider)
        {
            provider.Inject(this);
        }

        #region 訂單查詢 (清單/詳細/選項選單)

        /// <summary>
        /// 訂單清單 (公團 + 已加入的私團)
        /// </summary>
        /// <returns></returns>
        public List<OrderListModel> GetOrderList()
        {
            // 公團 (從資料庫搜尋)
            List<ViewOrder> publicOrder = _viewOrderRepository.GetAll().Where(x => 
                x.IsPublic
            ).ToList();

            // 私團 (從已加入的私團明細往回推私團資訊)
            List<ViewOrderDetail> privateOrderDetail = _viewOrderDetailRepository.GetAll().Where(x => x.DetailAccountID == _authService.UserID).ToList();
            List<ViewOrder> privateOrder = _viewOrderRepository.GetAll().Where(x =>
                !x.IsPublic
            ).AsEnumerable().Where(x =>
                privateOrderDetail.SelectProperty(y => y.OrderID).Contains(x.OrderID)
            ).ToList();

            // 組合公團 + 私團並轉型
            return publicOrder.Concat(privateOrder).OrderBy(x => 
                x.CloseTime
            ).Select(x => 
                new OrderListModel(x).SetButton(_authService.UserID)
            ).ToList();
        }

        /// <summary>
        /// 訂單詳細
        /// </summary>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        /// <exception cref="ApiException"></exception>
        public ViewOrderAndDetail GetOrder(Guid OrderID)
        {
            // 訂單原始資料
            ViewOrder order = _viewOrderRepository.GetAll().Where(x => x.OrderID == OrderID).FirstOrDefault() ?? throw new ApiException("訂單ID不存在", 400);

            // 訂單明細原始資料
            List<ViewOrderDetail> orderDetail = _viewOrderDetailRepository.GetAll().Where(x =>
                x.OrderID == order.OrderID
            ).ToList();

            // 訂單與訂單明細轉型
            OrderListModel orderList = new OrderListModel(order).SetButton(_authService.UserID);
            List<OrderDetailListModel> detailList = orderDetail.Select(x => new OrderDetailListModel(x).SetButton(_authService.UserID)).ToList();

            // 訂單明細依訂購者分群
            List<GroupOrderDetailModel> groupOrderDetail = _orderDetailRepository.GroupOrderDetailByName(detailList);

            // 組合訂單與訂單明細欄位
            return new ViewOrderAndDetail(orderList, groupOrderDetail);
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
                Store = _viewStoreRepository.GetAll().Select(x => new ResponseStoreListModel(x)).Select(x => new OptionsModel(x)).ToList()
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
            Order createOrder = _orderRepository.Create(new Order
            {
                O_office_id = RequestData.OfficeID,
                O_create_account_id = RequestData.CreateAccountID,
                O_store_id = RequestData.StoreID,
                O_no = CreateOrderNo(),
                O_type = RequestData.TypeID,
                O_arrival_time = RequestData.ArrivalTime,
                O_open_time = RequestData.OpenTime,
                O_close_time = RequestData.CloseTime,
                O_is_public = RequestData.IsPublic.HasValue && RequestData.IsPublic.Value,
            });

            // 開私團時自動將開團者加入，才能顯示在列表上
            JoinOrder(createOrder.O_id);

            // 開團的Line Notify通知
            _lineService.CreateOrderNotify(createOrder.O_id);

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
            _lineService.DelayNotify(OrderID);
        }

        /// <summary>
        /// 遲到抵達通知
        /// </summary>
        /// <param name="OrderID"></param>
        public void DelayArrivalNotify(Guid OrderID)
        {
            _lineService.DelayArrivalNotify(OrderID);
        }

        #endregion

        #region 訂單明細查詢 (訂單明細歷史紀錄)

        /// <summary>
        /// 訂單明細歷史紀錄
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        public List<ViewDetailHistory> GetOrderDetailHistory(Guid AccountID)
        {
            // 訂單明細原始資料
            List<ViewOrderDetail> orderDetail = _viewOrderDetailRepository.GetAll().Where(x =>
                x.DetailAccountID == AccountID
            ).ToList();

            // 訂單原始資料
            List<Guid> ids = orderDetail.SelectProperty(y => y.OrderID);
            List<ViewOrder> order = _viewOrderRepository.GetAll().Where(x =>
                ids.Contains(x.OrderID)
            ).AsEnumerable().ToList();

            // 訂單與訂單明細轉型
            List<OrderDetailListModel> detailList = orderDetail.Select(x => new OrderDetailListModel(x).SetButton(_authService.UserID)).ToList();
            List<OrderListModel> orderList = order.Select(x => new OrderListModel(x).SetButton(_authService.UserID)).ToList();

            return _orderDetailRepository.CombineDetailHistory(detailList, orderList);
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

        #region Line測試用方法

        public void TestOrderLine(Guid OrderID)
        {
            _lineService.CreateOrderNotify(OrderID);
            _lineService.DelayNotify(OrderID);
            _lineService.DelayArrivalNotify(OrderID);
        }

        #endregion
    }
}
