using System.Web;

namespace CodeShare.Libs.BaseProject
{
    public class CheckTokenController : BaseController
    {
        private readonly TokenManager _tokenManager;

        [Inject] protected readonly ITokenLogic _userInfo;

        /// <summary>
        /// 需登入的API呼叫使用
        /// </summary>
        public CheckTokenController(IServiceProvider provider) : base(provider)
        {
            provider.Inject(this);

            _tokenManager = new TokenManager(provider);
            SetLoginInfo();
        }

        /// <summary>
        /// 設定登入資訊
        /// </summary>
        private void SetLoginInfo()
        {
            // 取出網址參數
            var parameters = _httpContextAccessor.HttpContext.Request.Query;

            // 從網址參數 or Request Header 取Token判斷
            bool TokenSuccess;
            if (parameters.ContainsKey("Token"))
            {
                // GET 網址參數傳入的字串需要解碼
                TokenSuccess = _tokenManager.Check(HttpUtility.UrlDecode(parameters["Token"].ToString()), _userInfo.CheckTokenLogic);
            }
            else
            {
                TokenSuccess = _tokenManager.Check(_httpContextAccessor.HttpContext.Request, _userInfo.CheckTokenLogic);
            }
            if (!TokenSuccess)
            {
                throw new ApiException("您的請求未經授權，請確保登入或提供有效的授權憑證後再試一次", 401);
            }
        }
    }
}
