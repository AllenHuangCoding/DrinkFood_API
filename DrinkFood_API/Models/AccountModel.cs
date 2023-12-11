using DataBase.Entities;
using DataBase.View;
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

    #region Dashboard

    public class ResponseInfoCardModel
    {
        public List<InfoCardDataModel> Lunch { get; set; } = new();

        public List<InfoCardDataModel> Drink { get; set; } = new();

        public List<InfoCardDataModel> Teatime { get; set; } = new();

        public List<InfoCardDataModel> Other { get; set; } = new();
    }

    public class InfoCardDataModel
    {
        public string Title { get; set; }

        public string Main { get; set; }

        public string Info { get; set; }

        public InfoCardDataModel() { }

        public InfoCardDataModel(OrderListModel Entity)
        {
            string TitleDay;
            if (DateTime.Now.Date == Convert.ToDateTime(Entity.ArrivalTime).Date)
            {
                TitleDay = "今天";
            }
            else if (DateTime.Now.Date.AddDays(1) == Convert.ToDateTime(Entity.ArrivalTime).Date)
            {
                TitleDay = "明天";
            }
            else if (DateTime.Now.Date.AddDays(2) == Convert.ToDateTime(Entity.ArrivalTime).Date)
            {
                TitleDay = "後天";
            }
            else
            {
                TitleDay = Convert.ToDateTime(Entity.ArrivalTime).ToString("MM/dd");
            }
            Title = TitleDay + Entity.TypeDesc;
            Main = Entity.BrandName;
            
            if (DateTime.Now < Convert.ToDateTime(Entity.CloseTime))
            {
                Info = $"{Convert.ToDateTime(Entity.CloseTime):HH:mm} 結單";
            }
            else
            {
                Info = $"預計 {Convert.ToDateTime(Entity.ArrivalTime):HH:mm} 抵達";
            }
        }
    }

    public class ResponseTodayOrderModel
    {
        public Guid OrderDetailID {  get; set; }

        public string BrandName { get; set; }

        public string DrinkFoodName { get; set; }

        public string DetailRemark { get; set; }

        public int DrinkFoodPrice { get; set; }

        public int Quantity { get; set; }

        public ResponseTodayOrderModel(ViewOrderDetail Entity)
        {
            OrderDetailID = Entity.OrderDetailID;
            BrandName = Entity.BrandName;
            DrinkFoodName = Entity.DrinkFoodName!;
            DetailRemark = Entity.DetailRemark ?? "無";
            DrinkFoodPrice = Entity.DrinkFoodPrice!.Value;
            Quantity = Entity.Quantity!.Value;
        }
    }

    #endregion
}
