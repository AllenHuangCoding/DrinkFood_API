using DrinkFood_API.Exceptions;
using DrinkFood_API.Model;
using DrinkFood_API.Model.Login;
using DrinkFood_API.Service;
using DrinkFood_API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace DrinkFood_API.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// 紀錄登入資訊
        /// </summary>
        protected LoginInfoModel _loginInfo { get; set; } = new LoginInfoModel();

        public BaseController()
        {

        }

        public BaseController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 需登入的API呼叫使用，將登入資訊存放到 _loginInfo
        /// </summary>
        public BaseController(
            IHttpContextAccessor httpContextAccessor,
            PermissionService permissionService)
        {
            _httpContextAccessor = httpContextAccessor;

            //設定登入資訊
            SetLoginInfo(permissionService);
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

        /// <summary>
        /// 設定登入資訊
        /// </summary>
        protected void SetLoginInfo(PermissionService permissionService)
        {
            
        }

        /// <summary>
        /// 基本模組設定
        /// </summary>
        protected void SetModuleByPermission(string pagePermission)
        {
            // 設定模組
            SetHttpContextItem(ApiLogService.Module, pagePermission);
        }

        /// <summary>
        /// 設定此 api 需要紀錄log
        /// </summary>
        protected void SetApiLogNeedRecord(bool needRecord = true)
        {
            SetHttpContextItem(ApiLogService.NeedRecord, needRecord);
        }


    }
}
