using CodeShare.Libs.BaseProject;
using CodeShare.Libs.BaseProject.Extensions;
using DataBase.View;
using DrinkFood_API.Models;
using DrinkFood_API.Repository;

namespace DrinkFood_API.Services
{
    public class LineService : BaseService
    {
        private readonly LineNotify line;

        [Inject] private readonly ViewAccountRepository _viewAccountRepository;

        [Inject] private readonly OrderRepository _orderRepository;

        [Inject] private readonly ViewOrderDetailRepository _viewOrderDetailRepository;

        public LineService(IServiceProvider provider) : base(provider)
        {
            provider.Inject(this);

            // 產生Line Notify類別
            line = new(
                _configuration.GetSection("Line")["client_id"]!,
                _configuration.GetSection("Line")["client_secret"]!,
                _configuration.GetSection("Line")["redirect_uri"]!
            );
        }

        public string GetAccessToken(string code)
        {
            return line.GetAccessToken(code);
        }

        /// <summary>
        /// 公團開團通知
        /// </summary>
        /// <param name="OrderID"></param>
        /// <exception cref="ApiException"></exception>
        public void CreateOrderNotify(Guid OrderID)
        {
            // 取得有綁定、開啟Line的使用者資料
            List<ViewAccount> notifyAccount = GetNotifyAccount();

            // 取出剛新增成功的訂單資訊
            OrderListModel orderData = _orderRepository.GetOrderListData(OrderID);

            // 用訂單類型中文過濾有跟團類型的使用者
            switch (orderData.TypeDesc)
            {
                case "飲料":
                    notifyAccount = notifyAccount.Where(x => x.DrinkNotify).ToList();
                    break;
                case "午餐":
                case "下午茶":
                    notifyAccount = notifyAccount.Where(x => x.LunchNotify).ToList();
                    break;
                default:
                    break;
            }

            // 發送開團訊息
            string webUrl = _configuration.GetSection("Line")["web_url"]!;
            foreach (string item in notifyAccount.SelectProperty(x => x.LineID!))
            {
                line.Notify(item, 
                    string.Format(
                        $"開團通知\n" +
                        $"{orderData.TypeDesc}：{orderData.BrandStoreName}\n" +
                        $"抵達時間：{orderData.ArrivalTime}\n" +
                        $"截止時間：{orderData.CloseTime}\n" +
                        $"請大家直接去系統網址填寫訂單" +
                        $"{webUrl}/main/order/{orderData.OrderID}"
                    )
                );
            }
            
        }

        /// <summary>
        /// 遲到通知
        /// </summary>
        /// <param name="OrderID"></param>
        public void DelayNotify(Guid OrderID)
        {
            // 要發送Line訊息的Token
            List<string> notifyToken = GetOrderNotifyAccount(OrderID).SelectProperty(x => x.LineID!);

            // 取出遲到的訂單資訊
            OrderListModel orderData = _orderRepository.GetOrderListData(OrderID);

            // 發送遲到訊息
            foreach (string item in notifyToken)
            {
                line.Notify(
                    item, 
                    $"餐點遲到通知\n" +
                    $"{orderData.BrandStoreName}餐點會遲到，抵達會用訊息通知大家"
                );
            }
        }

        /// <summary>
        /// 遲到抵達通知
        /// </summary>
        /// <param name="OrderID"></param>
        public void DelayArrivalNotify(Guid OrderID)
        {
            // 要發送Line訊息的Token
            List<string> notifyToken = GetOrderNotifyAccount(OrderID).SelectProperty(x => x.LineID!);

            // 取出遲到抵達的訂單資訊
            OrderListModel orderData = _orderRepository.GetOrderListData(OrderID);

            // 發送遲到抵達訊息
            foreach (string item in notifyToken)
            {
                line.Notify(
                    item,
                    $"餐點抵達通知\n" +
                    $"請有訂{orderData.BrandStoreName}的大家可以來取餐"
                );
            }
        }

        #region 私有方法

        /// <summary>
        /// 取得訂單明細中有點餐 + 有打開Line通知的使用者
        /// </summary>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        private List<ViewAccount> GetOrderNotifyAccount(Guid OrderID)
        {
            // 取得訂單明細中有點餐的人
            List<ViewOrderDetail> viewOrderDetail = _viewOrderDetailRepository.GetAll().Where(x => 
                x.OrderID == OrderID && x.DrinkFoodID.HasValue
            ).ToList();

            // 轉換出使用者ID
            List<Guid> listAccountID = viewOrderDetail.SelectProperty(y => y.DetailAccountID);

            return _viewAccountRepository.GetAll().Where(x =>
                !string.IsNullOrWhiteSpace(x.LineID) && x.LineNotify &&
                listAccountID.Contains(x.AccountID)
            ).ToList();
        }

        /// <summary>
        /// 取得有打開Line通知的使用者
        /// </summary>
        /// <returns></returns>
        private List<ViewAccount> GetNotifyAccount()
        {
            return _viewAccountRepository.GetAll().Where(x =>
                !string.IsNullOrWhiteSpace(x.LineID) && x.LineNotify
            ).ToList();
        }

        #endregion
    }
}
