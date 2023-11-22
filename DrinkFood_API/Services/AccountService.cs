using DataBase.Entities;
using DrinkFood_API.Exceptions;
using DrinkFood_API.Model;
using DrinkFood_API.Models;
using DrinkFood_API.Repository;
using DrinkFood_API.Service;
using DrinkFood_API.Utility;

namespace DrinkFood_API.Services
{
    public class AccountService : BaseService
    {
        [Inject] private readonly AccountRepository _accountRepository;

        [Inject] private readonly CodeTableRepository _codeTableRepository;

        public AccountService(IServiceProvider provider)  : base()
        {
            provider.Inject(this);
        }

        public ResponseLoginModel Login(RequestLoginModel RequestData)
        {
            var user = _accountRepository.Exist(RequestData.Email, RequestData.Password) ?? throw new ApiException("帳號或密碼輸入錯誤", 400);
            
            return new ResponseLoginModel() { 
                Token = "", 
                AccountID = user.A_id, 
                Name = user.A_name, 
                Brief = user.A_brief 
            };
        }

        public List<ViewAccount> GetAccountList()
        {
            return _accountRepository.GetAccountList();
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


        public void CreateAccount(RequestCreateAccountModel RequestData)
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
    }
}
