namespace DrinkFood_API.Exceptions
{
    /// <summary>
    /// Api 錯誤回傳的例外處理
    /// </summary>
    [Serializable]
    public class ApiException : Exception
    {
        public int ErrorCode { get; set; } = 200;

        public ApiException()
        {
        }

        public ApiException(string message, int errorCode = 200) : base(message)
        {
            ErrorCode = errorCode;
        }

        public ApiException(string message, Exception innerExcep, int errorCode = 200) : base(message, innerExcep)
        {
            ErrorCode = errorCode;
        }
    }
}
