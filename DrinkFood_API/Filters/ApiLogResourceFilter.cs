using DrinkFood_API.Model;
using DrinkFood_API.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DrinkFood_API.Filters
{
    /// <summary>
    /// Api Log 紀錄處理
    /// </summary>
    public class ApiLogResourceFilter : IResourceFilter
    {
        private readonly ApiLogService _apiLogService;

        public ApiLogResourceFilter(ApiLogService apiLogService)
        {
            _apiLogService = apiLogService;
        }

        /// <summary>
        /// 從 context 組合成要記錄的資訊
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private ApiLogModel GetApiLogModelByFilterContext(ResourceExecutedContext context)
        {
            ApiLogModel model = null;

            // GET 行為不記錄 && 有設定模組的流程才紀錄
            if (context.Result != null &&
                context.HttpContext.Request.Method != HttpMethods.Get &&
                context.HttpContext.Items.ContainsKey(ApiLogService.Module))
            {
                model = InsertLog(context);
            }

            return model;
        }

        public ApiLogModel InsertLog(ResourceExecutedContext context)
        {
            ApiLogModel model = null;

            // 加入裝置資訊
            string os = null;
            string device = null;
            string browser = null;
            var userAgent = context.HttpContext.Request.Headers["User-Agent"];
            var parser = UAParser.Parser.GetDefault();
            var info = parser.Parse(userAgent);
            if (info != null)
            {
                os = info.OS.ToString();
                device = info.Device.ToString();
                browser = info.UA.ToString();
            }

            if (typeof(JsonResult) == context.Result.GetType())
            {
                JsonResult result = (JsonResult)context.Result;

                // 查詢行為不紀錄 (為 ResponseData 的皆不記錄，除了特殊行為)
                bool needRecord = false;
                if (typeof(ResponseModel) == result.Value.GetType())
                {
                    needRecord = true;
                }
                else if (context.HttpContext.Items.ContainsKey(ApiLogService.NeedRecord))
                {
                    // 特殊行為透過此欄位卡控是否紀錄
                    needRecord = (bool)context.HttpContext.Items[ApiLogService.NeedRecord];
                }

                if (needRecord)
                {
                    var response = (ResponseModel)result.Value;

                    string body = context.HttpContext.Items.ContainsKey(ApiLogService.Body) ?
                            context.HttpContext.Items[ApiLogService.Body].ToString() : null;

                    string accountId = context.HttpContext.Items.Where(p => p.Key.ToString() == ApiLogService.AccountId).Select(p => p.Value.ToString()).FirstOrDefault();

                    model = new ApiLogModel()
                    {
                        AccountId = accountId,
                        Method = context.HttpContext.Request.Method,
                        Path = context.HttpContext.Request.Path.ToString(),
                        StatusCode = response.Code,
                        Success = response.Success,
                        Message = response.Message,
                        Module = context.HttpContext.Items[ApiLogService.Module].ToString(),
                        Body = body,
                        IpAddress = context.HttpContext.Connection.RemoteIpAddress.ToString(),
                        Os = os,
                        Device = device,
                        Browser = browser,
                    };
                }
            }
            else if (typeof(FileContentResult) == context.Result.GetType())
            {
                // 匯出檔案動作也需要紀錄
                FileContentResult result = (FileContentResult)context.Result;

                string body = context.HttpContext.Items.ContainsKey(ApiLogService.Body) ?
                        context.HttpContext.Items[ApiLogService.Body].ToString() : null;

                model = new ApiLogModel()
                {
                    AccountId = context.HttpContext.User.Identity.Name,
                    Method = context.HttpContext.Request.Method,
                    Path = context.HttpContext.Request.Path.ToString(),
                    StatusCode = context.HttpContext.Response.StatusCode,
                    Success = true,
                    Message = result.FileDownloadName,
                    Module = context.HttpContext.Items[ApiLogService.Module].ToString(),
                    Body = body,
                    IpAddress = context.HttpContext.Connection.RemoteIpAddress.ToString(),
                    Os = os,
                    Device = device,
                    Browser = browser,
                };
            }
            return model;
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            try
            {
                // 紀錄 response
                var model = GetApiLogModelByFilterContext(context);
                if (model != null)
                {
                    _apiLogService.AddApiLog(model);
                }
            }
            catch (Exception)
            {
                //line錯誤異常警告
            }
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            //進行判斷條件等事項

        }
    }
}
