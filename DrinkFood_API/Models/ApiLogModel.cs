namespace DrinkFood_API.Model
{
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
