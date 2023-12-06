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

        public AccountService(IServiceProvider provider)  : base(provider)
        {
            provider.Inject(this);
        }

        public List<ViewAccount> GetAccountList()
        {
            if (_authService.IsAdmin)
            {
                return _accountRepository.GetAccountList();
            }
            return new List<ViewAccount>();
        }


        public ResponseProfileDialogOptions GetProfileDialogOptions()
        {
            return new ResponseProfileDialogOptions
            {
                LunchPayment = _codeTableRepository.FindAll(x => x.CT_type == "LunchPayment").OrderBy(x => x.CT_order).Select(x => new OptionsModel(x)).ToList(),
                DrinkPayment = _codeTableRepository.FindAll(x => x.CT_type == "DrinkPayment").OrderBy(x => x.CT_order).Select(x => new OptionsModel(x)).ToList(),
            };
        }

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
    }
}
