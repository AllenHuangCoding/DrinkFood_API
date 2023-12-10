using CodeShare.Libs.BaseProject;
using DataBase.Entities;
using DrinkFood_API.Repository;

namespace DrinkFood_API.Services
{

    public class AuthService : IAuthService
    {
        public Guid UserID { get; set; }

        public bool IsAdmin { get; set; }

        [Inject] private readonly AccountLoginRepository _accountLoginRepository;

        [Inject] private readonly AccountRepository _accountRepository;

        public AuthService(IServiceProvider provider)
        {
            provider.Inject(this);
        }

        /// <summary>
        /// 實作檢查Token資料庫邏輯
        /// </summary>
        /// <param name="token"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        /// <exception cref="ApiException"></exception>
        public bool CheckTokenLogic(string token, Payload payload)
        {
            _ = _accountLoginRepository.Exist(token);

            if (payload.UserID == Guid.Empty)
            {

            }

            UserID = payload.UserID;
            Account account = _accountRepository.Exist(payload.UserID) ?? throw new ApiException("使用者不存在", 400);
            IsAdmin = account.A_is_admin;
            return true;
        }

        /// <summary>
        /// 實作檢查管理員身分 (非管理員噴出ApiException)
        /// </summary>
        public void CheckAdmin()
        {
            if (!IsAdmin)
            {
                throw new ApiException("非管理員權限", 400);
            }
        }
    }
}
