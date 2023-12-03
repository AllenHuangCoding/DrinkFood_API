using CodeShare.Libs.BaseProject;
using DrinkFood_API.Repository;

namespace DrinkFood_API.Services
{

    public class AuthService : BaseService, ITokenLogic
    {
        public Guid UserID { get; set; }

        [Inject] private readonly AccountLoginRepository _accountLoginRepository;

        public AuthService(IServiceProvider provider) : base(provider)
        {
            provider.Inject(this);
        }

        public bool CheckTokenLogic(string token, Payload payload)
        {
            // 驗證時效性邏輯

            _ = _accountLoginRepository.Exist(token);

            UserID = payload.UserID;

            return true;
        }
    }
}
