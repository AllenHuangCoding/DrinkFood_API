using CodeShare.Libs.BaseProject;
using DataBase.Entities;
using DrinkFood_API.Models;
using DrinkFood_API.Repository;

namespace DrinkFood_API.Services
{
    public class LoginService : BaseService
    {
        [Inject] private readonly AccountRepository _accountRepository;

        [Inject] private readonly AccountLoginRepository _accountLoginRepository;

        private readonly TokenManager _tokenManager;

        public LoginService(IServiceProvider provider) : base(provider)
        {
            provider.Inject(this);
            _tokenManager = new TokenManager(provider);
        }

        public ResponseLoginModel? Login(RequestLoginModel RequestData)
        {
            ResponseLoginModel? response = null;

            var account = _accountRepository.Exist(RequestData.Email, RequestData.Password) ?? throw new ApiException("帳號或密碼輸入錯誤", 400);
            
            _tokenManager.Create(account.A_id, (token) =>
            {
                _accountLoginRepository.PatchDelete(account.A_id);

                _accountLoginRepository.Create(new AccountLogin()
                {
                    AL_account_id = account.A_id,
                    AL_token = token,
                });

                response = new()
                {
                    AccountID = account.A_id,
                    Token = $"Bearer {token}",
                    Name = account.A_name,
                    Brief = account.A_brief,
                    IsAdmin = account.A_is_admin
                };
            });

            return response;
        }
    }
}
