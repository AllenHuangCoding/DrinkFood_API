using DataBase.Entities;
using DrinkFood_API.Utility;

namespace DrinkFood_API.Models
{
    /// <summary>
    /// 登入參數
    /// </summary>
    public class RequestLoginModel
    {
        /// <summary>
        /// 員工信箱
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        /// 密碼
        /// </summary>
        public string Password { get; set; }
    }

    /// <summary>
    /// 登入回傳結果
    /// </summary>
    public class ResponseLoginModel
    {
        public Guid AccountID { get; set; }

        /// <summary>
        /// Token字串
        /// </summary>
        public string Token { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? Brief { get; set; }

        public bool IsAdmin { get; set; }
    }

    public class ViewAccount
    {
        public Guid AccountID { get; set; }

        public string Name { get; set; } = null!;

        public string? Brief { get; set; }

        public string Email { get; set; } = null!;

        public string? LineID { get; set; }

        public bool LunchNotify { get; set; }

        public bool DrinkNotify { get; set; }

        public int CloseNotify { get; set; }

        public Guid? DefaultLunchPayment {  get; set; }

        public string? DefaultLunchPaymentDesc { get; set; }

        public Guid? DefaultDrinkPayment { get; set; }

        public string? DefaultDrinkPaymentDesc { get; set; }

        public bool IsAdmin { get; set; }
    }

    public class ResponseAccountListModel
    {
        public Guid AccountID { get; set; }

        public string Name { get; set; } = null!;

        public string? Brief { get; set; }

        public string Email { get; set; } = null!;
    }

    public class AccountShareColumn
    {
        public string? Brief { get; set; }

        public Guid? LunchDefaultPayment { get; set; }

        public Guid? DrinkDefaultPayment { get; set; }

        public bool LunchNotify { get; set; }

        public bool DrinkNotify { get; set; }

        public int CloseNotify { get; set; }
    }

    public class RequestUpdateProfileModel : AccountShareColumn
    {
        
    }

    public class UpdateProfileModel : AccountShareColumn
    {
        public Guid AccountID { get; set; }
    }

    public class RequestCreateAccountModel : AccountShareColumn
    {
        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;
    }


    public class ResponseProfileDialogOptions
    {
        public List<OptionsModel> LunchPayment { get; set; }

        public List<OptionsModel> DrinkPayment { get; set; }
    }

    #region

    public class RequestBindLineModel
    {
        public string code { get; set; } = null!;

        public Guid state { get; set; }
    }

    #endregion
}
