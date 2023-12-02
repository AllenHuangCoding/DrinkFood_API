namespace DrinkFood_API.Utility
{
    /// <summary>
    /// Token
    /// </summary>
    public class TokenModel
    {
        /// <summary>
        /// token
        /// </summary>
        public string Token { get; set; }
    }

    /// <summary>
    /// 使用者資訊
    /// </summary>
    public class Payload
    {
        /// <summary>
        /// 使用者Id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 創建時間
        /// </summary>
        public DateTime CreateDate { get; set; }

        public Payload()
        {
            CreateDate = DateTime.Now;
        }
    }
}
