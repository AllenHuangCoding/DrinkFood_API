using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataBase.Entities
{
    /// <summary>
    /// 操作Log
    /// </summary>
    [Table("ApiLog")]
    public partial class ApiLog
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long No { get; set; }
        /// <summary>
        /// 模組
        /// </summary>
        public string Module { get; set; }
        /// <summary>
        /// 方法
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// 路徑
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 代碼
        /// </summary>
        public int Status_code { get; set; }
        /// <summary>
        /// 結果
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 訊息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 請求者Id
        /// </summary>
        public string Account_id { get; set; }
        /// <summary>
        /// 來源內容
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// 請求地址
        /// </summary>
        public string Ip_address { get; set; }
        /// <summary>
        /// 系統
        /// </summary>
        public string Os { get; set; }
        /// <summary>
        /// 設備
        /// </summary>
        public string Device { get; set; }
        /// <summary>
        /// 瀏覽器
        /// </summary>
        public string Browser { get; set; }
        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime Create_time { get; set; }
        public ApiLog()
        {
            Create_time = DateTime.Now;
        }
    }
}
