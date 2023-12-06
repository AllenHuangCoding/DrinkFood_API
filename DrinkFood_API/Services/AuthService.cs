using CodeShare.Libs.BaseProject;
using DataBase.Entities;
using DrinkFood_API.Repository;

namespace DrinkFood_API.Services
{

    public class AuthService : BaseService, ITokenLogic
    {
        public Guid UserID { get; set; }

        public bool IsAdmin { get; set; }

        [Inject] private readonly AccountLoginRepository _accountLoginRepository;

        [Inject] private readonly AccountRepository _accountRepository;

        public AuthService(IServiceProvider provider) : base(provider)
        {
            provider.Inject(this);
        }

        public bool CheckTokenLogic(string token, Payload payload)
        {
            _ = _accountLoginRepository.Exist(token);

            if (payload.UserID == default)
            {

            }

            UserID = payload.UserID;
            Account account = _accountRepository.Exist(payload.UserID) ?? throw new ApiException("使用者不存在", 400);
            IsAdmin = account.A_is_admin;
            return true;
        }
    }
}
