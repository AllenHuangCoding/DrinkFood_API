using DataBase.Entities;
using DrinkFood_API.Exceptions;
using DrinkFood_API.Models;
using DrinkFood_API.Repository;
using DrinkFood_API.Service;

namespace DrinkFood_API.Services
{
    public class AccountService : BaseService
    {
        [Inject] private readonly AccountRepository _accountRepository;

        public AccountService(IServiceProvider provider)  : base()
        {
            provider.Inject(this);
        }

        public ResponseLoginModel? Login(RequestLoginModel Request)
        {
            _ = _accountRepository.Exist(Request.Number, Request.Password) ?? throw new ApiException("帳號或密碼輸入錯誤", 400);
            return new ResponseLoginModel() { Token = "" };
        }

        public ResponseProfileModel? GetProfile(Guid AccountID)
        {
            var account = _accountRepository.Exist(AccountID) ?? throw new ApiException("使用者ID不存在", 400);
            return new ResponseProfileModel(account);
        }

        public void UpdateProfile(Guid AccountID, RequestUpdateProfileModel Request)
        {
            _accountRepository.UpdateProfile(new UpdateProfileModel
            {
                AccountID = AccountID,
                AccountName = Request.Name,
                AccountBrief = Request.Brief,
            });
        }
    }
}
