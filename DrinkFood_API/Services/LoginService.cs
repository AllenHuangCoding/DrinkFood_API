using DrinkFood_API.Exceptions;
using DrinkFood_API.Models;
using DrinkFood_API.Repository;
using DrinkFood_API.Service;

namespace DrinkFood_API.Services
{
    public class LoginService : BaseService
    {
        [Inject] private readonly AccountRepository _accountRepository;

        [Inject] private readonly AuthService _authService;

        public LoginService(IServiceProvider provider) : base()
        {
            provider.Inject(this);
        }

        public ResponseLoginModel Login(RequestLoginModel RequestData)
        {
            var user = _accountRepository.Exist(RequestData.Email, RequestData.Password) ?? throw new ApiException("帳號或密碼輸入錯誤", 400);
            _authService.CreateToken(user.A_id, (token) =>
            {
                //Account account = _context.Account.Where(x => x.A_id == accountID).First();
                //ResponseLoginModel ResponseModel = new ResponseLoginModel
                //{
                //    Token = string.Format("Bearer {0}", Token),
                //    AccountId = account.A_id,
                //    AccountName = account.A_name,
                //    AccountNumber = account.A_number,
                //    IsAdmin = account.A_isAdmin,
                //    NeedChange = account.A_change_password
                //};
                //account.A_update = TokenCreateDate;

                //_context.AccountLogin.Add(new AccountLogin()
                //{
                //    AL_account_id = accountID,
                //    AL_token = ResponseModel.Token,
                //    AL_validity = TokenCreateDate.AddHours(2),
                //    AL_ip = ip,
                //    AL_device = "Web"
                //});
                //_context.SaveChanges();
            });

            return new ResponseLoginModel()
            {
                Token = "",
                AccountID = user.A_id,
                Name = user.A_name,
                Brief = user.A_brief
            };
        }
    }
}
