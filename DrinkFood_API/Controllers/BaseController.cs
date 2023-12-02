using DrinkFood_API.Service;
using Microsoft.AspNetCore.Mvc;

namespace DrinkFood_API.Controllers
{
    public class BaseController : Controller
    {
        [Inject] protected readonly IHttpContextAccessor _httpContextAccessor;

        public BaseController(IServiceProvider provider)
        {
            provider.Inject(this);
        }

        /// <summary>
        /// 設定此 api 需要紀錄log
        /// </summary>
        protected void SetApiLogNeedRecord(bool needRecord = true)
        {
            SetHttpContextItem(ApiLogService.NeedRecord, needRecord);
        }

        /// <summary>
        /// 設定自訂資訊到 http context 中
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        protected void SetHttpContextItem(string key, object value)
        {
            var items = _httpContextAccessor.HttpContext.Items;
            if (items.ContainsKey(key))
            {
                items[key] = value;
            }
            else
            {
                items.Add(key, value);
            }
        }
    }
}
