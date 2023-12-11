using CodeShare.Libs.BaseProject;
using DataBase.Entities;
using DataBase.View;
using DrinkFood_API.Models;
using DrinkFood_API.Repository;

namespace DrinkFood_API.Services
{
    public class HomeService : BaseService
    {
        [Inject] private readonly ViewOrderRepository _viewOrderRepository;

        [Inject] private readonly ViewOrderDetailRepository _viewOrderDetailRepository;

        [Inject] private readonly CodeTableRepository _codeTableRepository;

        [Inject] private readonly IAuthService _authService;

        [Inject] private readonly OrderService _orderService;

        public HomeService(IServiceProvider provider) : base(provider)
        {
            provider.Inject(this);
        }

        #region 首頁三區塊

        public ResponseInfoCardModel GetInfoCard()
        {
            // 查詢未來一周的訂單
            DateTime sd = DateTime.Now.Date;
            DateTime ed = DateTime.Now.Date.AddDays(7);

            // 必須使用GetOrderList才有私團的邏輯
            List<OrderListModel> sevenDaysOrder = _orderService.GetOrderList().Where(x =>
                sd <= Convert.ToDateTime(x.ArrivalTime) && Convert.ToDateTime(x.ArrivalTime) <= ed
            ).ToList();

            // 將一周的訂單分成午餐、飲料、下午茶類型
            List<CodeTable> orderType = _codeTableRepository.FindAll(x => x.CT_type == "OrderType").ToList();

            // 未來一周午餐訂單
            CodeTable lunchType = orderType.First(x => x.CT_desc == "午餐");
            List<OrderListModel> lunchOrder = sevenDaysOrder.Where(x => x.Type == lunchType.CT_id).ToList();

            // 未來一周飲料訂單
            CodeTable drinkType = orderType.First(x => x.CT_desc == "飲料");
            List<OrderListModel> drinkOrder = sevenDaysOrder.Where(x => x.Type == drinkType.CT_id).ToList();

            // 未來一周下午茶訂單
            CodeTable teatimeType = orderType.First(x => x.CT_desc == "下午茶");
            List<OrderListModel> teatimeOrder = sevenDaysOrder.Where(x => x.Type == teatimeType.CT_id).ToList();

            ResponseInfoCardModel response = new()
            {
                Lunch = lunchOrder.Select(x => new InfoCardDataModel(x)).ToList(),
                Drink = drinkOrder.Select(x => new InfoCardDataModel(x)).ToList(),
                Teatime = teatimeOrder.Select(x => new InfoCardDataModel(x)).ToList(),
                Other = new List<InfoCardDataModel>() {
                    new() { Title = "訊息", Main = "0則", Info = "0則" }
                }
            };

            return response;
        }

        public List<ResponseTodayOrderModel> GetTodayOrder()
        {
            // 設定今日時間區間
            DateTime sd = DateTime.Now.Date;
            DateTime ed = DateTime.Now.Date.AddDays(1);

            // 取得原始資料
            List<ViewOrderDetail> orderDetail = _viewOrderDetailRepository.FindAll(x =>
                x.DetailAccountID == _authService.UserID &&
                x.DrinkFoodID.HasValue && x.DrinkFoodPrice.HasValue && x.Quantity.HasValue &&
                !string.IsNullOrWhiteSpace(x.DrinkFoodName) &&
                sd <= x.ArrivalTime && x.ArrivalTime < ed
            ).OrderBy(x => x.ArrivalTime).ToList();

            // 轉型
            return orderDetail.Select(x => new ResponseTodayOrderModel(x)).ToList();
        }


        #endregion

    }
}
