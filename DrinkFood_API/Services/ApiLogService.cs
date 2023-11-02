using DataBase;
using DataBase.Entities;
using DrinkFood_API.Model;

namespace DrinkFood_API.Service
{
    public class ApiLogService : BaseService
    {
        public ApiLogService() : base()
        {
        }

        /// <summary>
        /// 放在 context item 的模組 key
        /// </summary>
        public const string Module = "module";

        /// <summary>
        /// 放在 context item 的模組的預設值
        /// </summary>
        public const string ModuleDefault = "other";

        /// <summary>
        /// 設定是否記錄 log 的 key
        /// </summary>
        public const string NeedRecord = "needRecord";

        /// <summary>
        /// 設定 body 的 key
        /// </summary>
        public const string Body = "body";

        /// <summary>
        /// 設定 accountId 的 key
        /// </summary>
        public const string AccountId = "accountId";

        #region 新增

        /// <summary>
        /// 新增 API LOG
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ApiLog AddApiLog(ApiLogModel model)
        {
            ApiLog newItem = new ApiLog()
            {
                Module = model.Module,
                Method = model.Method,
                Path = model.Path,
                Status_code = model.StatusCode,
                Success = model.Success,
                Message = model.Message,
                Account_id = model?.AccountId ?? "default",
                Body = model.Body,
                Ip_address = model.IpAddress,
                Os = model.Os,
                Device = model.Device,
                Browser = model.Browser,
                Create_time = DateTime.Now
            };
            //_dbContext.ApiLog.Add(newItem);
            //_dbContext.SaveChanges();

            return newItem;
        }

        #endregion

    }
}
