namespace CodeShare.Libs.BaseProject
{
    public interface IApiLogService
    {
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

        void AddApiLog(ApiLogModel model);
    }

    /// <summary>
    /// 例外Log紀錄
    /// </summary>
    public class ApiLogModel
    {
        public string Module { get; set; }
        public string Method { get; set; }
        public string Path { get; set; }
        public int StatusCode { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public string AccountId { get; set; }
        public string Body { get; set; }
        public string IpAddress { get; set; }
        public string Os { get; set; }
        public string Device { get; set; }
        public string Browser { get; set; }
    }
}
