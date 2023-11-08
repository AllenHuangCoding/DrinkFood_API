using DataBase.Entities;

namespace DrinkFood_API.Models
{
    /// <summary>
    /// 登入參數
    /// </summary>
    public class RequestLoginModel
    {
        /// <summary>
        /// 員工編號
        /// </summary>
        public required string Number { get; set; }
        
        /// <summary>
        /// 密碼
        /// </summary>
        public required string Password { get; set; }
    }

    /// <summary>
    /// 登入回傳結果
    /// </summary>
    public class ResponseLoginModel
    {
        /// <summary>
        /// Token字串
        /// </summary>
        public required string Token { get; set; }
    }

    public class ResponseAccountListModel
    {
        public Guid AccountID { get; set; }

        public string Name { get; set; } = null!;

        public string? Brief { get; set; }

        public string Email { get; set; } = null!;
    }

    public class RequestUpdateProfileModel
    {
        public string Name { get; set;}

        public string Brief { get; set;}
    }

    public class UpdateProfileModel 
    {
        public Guid AccountID { get; set; }

        public string AccountName { get; set; }

        public string AccountBrief { get; set; }
    }

    public class ResponseProfileModel
    {
        public Guid AccountID { get; set; }

        public string Name { get; set; } 

        public string? Brief { get; set; }

        public string Email { get; set; }

        public ResponseProfileModel(Account Entity)
        {
            AccountID = Entity.A_id;
            Name = Entity.A_name;
            Brief = Entity.A_brief;
            Email = Entity.A_email;
        }
    }
}
