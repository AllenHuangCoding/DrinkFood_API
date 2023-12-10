using DataBase.Entities;
using CodeShare.Libs.BaseProject;
using DrinkFood_API.Models;
using DrinkFood_API.Repository;
using DrinkFood_API.Utility;
using DataBase.View;

namespace DrinkFood_API.Services
{
    public class AccountService : BaseService
    {
        [Inject] private readonly AccountRepository _accountRepository;

        [Inject] private readonly ViewAccountRepository _viewAccountRepository;

        [Inject] private readonly ViewOrderRepository _viewOrderRepository;

        [Inject] private readonly ViewOrderDetailRepository _viewOrderDetailRepository;

        [Inject] private readonly CodeTableRepository _codeTableRepository;

        [Inject] private readonly IAuthService _authService;

        [Inject] private readonly LineService _lineService;

        public AccountService(IServiceProvider provider)  : base(provider)
        {
            provider.Inject(this);
        }

        #region 首頁三區塊

        public ResponseInfoCardModel GetInfoCard()
        {
            // 需要改成有私團的版本

            // 查詢未來一周的訂單
            DateTime sd = DateTime.Now.Date;
            DateTime ed = DateTime.Now.Date.AddDays(7);

            List<ViewOrder> sevenDaysOrder = _viewOrderRepository.FindAll(x =>
                sd <= x.ArrivalTime && x.ArrivalTime <= ed
            ).ToList();

            // 將一周的訂單分成午餐、飲料、下午茶類型
            List<CodeTable> orderType = _codeTableRepository.FindAll(x => x.CT_type == "OrderType").ToList();

            // 未來一周午餐訂單
            CodeTable lunchType = orderType.First(x => x.CT_desc == "午餐");
            List<ViewOrder> lunchOrder = sevenDaysOrder.Where(x => x.Type == lunchType.CT_id).ToList();

            // 未來一周飲料訂單
            CodeTable drinkType = orderType.First(x => x.CT_desc == "飲料");
            List<ViewOrder> drinkOrder = sevenDaysOrder.Where(x => x.Type == drinkType.CT_id).ToList();

            // 未來一周下午茶訂單
            CodeTable teatimeType = orderType.First(x => x.CT_desc == "下午茶");
            List<ViewOrder> teatimeOrder = sevenDaysOrder.Where(x => x.Type == teatimeType.CT_id).ToList();

            ResponseInfoCardModel response = new()
            {
                Lunch = lunchOrder.Select(x => new InfoCardDataModel(x)).ToList(),
                Drink = drinkOrder.Select(x => new InfoCardDataModel(x)).ToList(),
                Teatime = teatimeOrder.Select(x => new InfoCardDataModel(x)).ToList(),
                Other = new() { Title = "許願池", Main = "尚未開放", Info = "尚未開放" }
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
            ).ToList();

            // 轉型
            return orderDetail.Select(x => new ResponseTodayOrderModel(x)).ToList();
        }


        #endregion

        #region 普通使用者 (查詢個人資料/修改個人資料/午餐與飲料付款方式選單)

        /// <summary>
        /// 查詢個人資料
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ApiException"></exception>
        public ViewAccount GetProfile()
        {
            return _viewAccountRepository.FindAll(x => x.AccountID == _authService.UserID).FirstOrDefault() ?? throw new ApiException("使用者ID不存在", 400);
        }

        /// <summary>
        /// 修改個人資料
        /// </summary>
        /// <param name="AccountID"></param>
        /// <param name="RequestData"></param>
        /// <exception cref="ApiException"></exception>
        public void UpdateProfile(Guid AccountID, RequestUpdateProfileModel RequestData)
        {
            if (_authService.IsAdmin || AccountID == _authService.UserID)
            {
                _accountRepository.UpdateProfile(new UpdateProfileModel
                {
                    AccountID = AccountID,
                    Brief = RequestData.Brief,
                    LunchDefaultPayment = RequestData.LunchDefaultPayment,
                    DrinkDefaultPayment = RequestData.DrinkDefaultPayment,
                    LunchNotify = RequestData.LunchNotify,
                    DrinkNotify = RequestData.DrinkNotify,
                    CloseNotify = RequestData.CloseNotify,
                });
            }
            else
            {
                throw new ApiException("非管理員權限", 400);
            }
        }

        /// <summary>
        /// 午餐與飲料付款方式選單
        /// </summary>
        /// <returns></returns>
        public ResponseProfileDialogOptions GetProfileDialogOptions()
        {
            return new ResponseProfileDialogOptions
            {
                LunchPayment = _codeTableRepository.FindAll(x => x.CT_type == "LunchPayment").OrderBy(x => x.CT_order).Select(x => new OptionsModel(x)).ToList(),
                DrinkPayment = _codeTableRepository.FindAll(x => x.CT_type == "DrinkPayment").OrderBy(x => x.CT_order).Select(x => new OptionsModel(x)).ToList(),
            };
        }

        #endregion

        #region Line相關功能

        public void BindLine(Guid AccountID, RequestBindLineModel RequestData)
        {
            if (AccountID != RequestData.state)
            {
                throw new ApiException("操作使用者與綁定使用者不同", 400);
            }

            var account = _accountRepository.GetById(AccountID) ?? throw new ApiException("使用者ID不存在", 400);
            account.A_line_id = _lineService.GetAccessToken(RequestData.code);
            account.A_update = DateTime.Now;
            _accountRepository.Update(AccountID, account);
        }   

        public void UnbindLine(Guid AccountID)
        {
            var account = _accountRepository.GetById(AccountID) ?? throw new ApiException("使用者ID不存在", 400);
            account.A_line_id = null;
            account.A_update = DateTime.Now;
            _accountRepository.Update(AccountID, account);
        }

        #endregion

        #region 管理者獨有功能 (使用者清單/新增使用者)

        /// <summary>
        /// 使用者清單
        /// </summary>
        /// <returns></returns>
        public List<ViewAccount> GetAccountList()
        {
            _authService.CheckAdmin();
            return _viewAccountRepository.GetAll().ToList();
        }

        /// <summary>
        /// 新增使用者
        /// </summary>
        /// <param name="RequestData"></param>
        /// <exception cref="ApiException"></exception>
        public void CreateAccount(RequestCreateAccountModel RequestData)
        {
            _authService.CheckAdmin();

            _accountRepository.Create(new Account
            {
                A_name = RequestData.Name,
                A_brief = RequestData.Brief,
                A_email = RequestData.Email,
                A_password = RequestData.Email,
                A_lunch_notify = RequestData.LunchNotify,
                A_drink_notify = RequestData.DrinkNotify,
                A_close_notify = RequestData.CloseNotify,
                A_default_lunch_payment = RequestData.LunchDefaultPayment,
                A_default_drink_payment = RequestData.DrinkDefaultPayment,
            });
        }

        #endregion

    }
}
