using DataBase.Entities;
using CodeShare.Libs.BaseProject;
using DrinkFood_API.Models;
using DrinkFood_API.Repository;
using DrinkFood_API.Utility;

namespace DrinkFood_API.Services
{
    public class AccountService : BaseService
    {
        [Inject] private readonly AccountRepository _accountRepository;

        [Inject] private readonly CodeTableRepository _codeTableRepository;

        [Inject] private readonly AuthService _authService;

        [Inject] private readonly LineService _lineService;

        public AccountService(IServiceProvider provider)  : base(provider)
        {
            provider.Inject(this);
        }

        #region 普通使用者 (查詢個人資料/修改個人資料/午餐與飲料付款方式選單)

        /// <summary>
        /// 查詢個人資料
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ApiException"></exception>
        public ViewAccount GetProfile()
        {
            return _accountRepository.GetViewAccount().Where(x => x.AccountID == _authService.UserID).FirstOrDefault() ?? throw new ApiException("使用者ID不存在", 400);
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

            var account = _accountRepository.Exist(AccountID) ?? throw new ApiException("使用者ID不存在", 400);
            account.A_line_id = _lineService.GetAccessToken(RequestData.code);
            _accountRepository.Update(AccountID, account);
        }   

        public void UnbindLine(Guid AccountID)
        {
            var account = _accountRepository.Exist(AccountID) ?? throw new ApiException("使用者ID不存在", 400);
            account.A_line_id = null;
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
            if (_authService.IsAdmin)
            {
                return _accountRepository.GetViewAccount().ToList();
            }
            return new List<ViewAccount>();
        }

        /// <summary>
        /// 新增使用者
        /// </summary>
        /// <param name="RequestData"></param>
        /// <exception cref="ApiException"></exception>
        public void CreateAccount(RequestCreateAccountModel RequestData)
        {
            if (_authService.IsAdmin)
            {
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
            else
            {
                throw new ApiException("非管理員權限", 400);
            }
        }

        #endregion

    }
}
