using DrinkFood_API.Exceptions;
using DrinkFood_API.Services;
using System.Web;

namespace DrinkFood_API.Controllers
{
    public class CheckTokenController : BaseController
    {
        [Inject] protected AuthService _authService;

        /// <summary>
        /// 需登入的API呼叫使用
        /// </summary>
        public CheckTokenController(IServiceProvider provider) : base(provider)
        {
            SetLoginInfo();
        }

        /// <summary>
        /// 設定登入資訊
        /// </summary>
        protected void SetLoginInfo()
        {
            // 取出網址參數
            var parameters = _httpContextAccessor.HttpContext.Request.Query;

            // 從網址參數 or Request Header 取Token判斷
            bool TokenSuccess;
            if (parameters.ContainsKey("Token"))
            {
                // GET 網址參數傳入的字串需要解碼
                TokenSuccess = _authService.CheckToken(HttpUtility.UrlDecode(parameters["Token"].ToString()));
            }
            else
            {
                TokenSuccess = _authService.CheckToken(_httpContextAccessor.HttpContext.Request);
            }
            if (!TokenSuccess)
            {
                throw new ApiException("您的請求未經授權，請確保登入或提供有效的授權憑證後再試一次", 401);
            }
        }
    }
}
