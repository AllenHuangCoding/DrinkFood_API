using CodeShare.Libs.BaseProject.Extensions;
using DataBase.Entities;
using DrinkFood_API.Utility;
using Microsoft.Identity.Client;
using Org.BouncyCastle.Bcpg;

namespace DrinkFood_API.Models
{
    public class RequestGetMyOrderListModel
    {
    }

    /// <summary>
    /// 新增訂單前的下拉選單
    /// </summary>
    public class ResponseOrderDialogOptions
    {
        public List<OptionsModel> Office { get; set; }
        public List<OptionsModel> Type { get; set; }
        public List<OptionsModel> Store { get; set; }
    }

    public class RequestPostOrderModel
    {
        public required Guid OfficeID { get; set; }

        public required Guid CreateAccountID { get; set; }

        public required Guid StoreID  { get; set; }

        public required Guid TypeID { get; set; }

        public required DateTime ArrivalTime { get; set; }

        public required DateTime OpenTime { get; set; }

        public required DateTime CloseTime { get; set; }

        public required bool IsPublic { get; set; }

    }

    public class RequestPutArrivalTimeModel
    {
        public DateTime ArrivalTime { get; set; }
    }

    public class RequestPutCloseTimeModel
    {
        public DateTime CloseTime { get; set; }
    }

    public class PutOrderTimeModel 
    {
        public Guid OrderID { get; set; }   

        public DateTime DrinkTime { get; set; }

        public DateTime CloseTime { get; set; }
    }

    public class ViewOrder
    {
        public Guid OrderID { get; set; }

        public Guid OwnerID { get; set; }

        public string OwnerName { get; set; } = null!;

        public string OrderNo { get; set; } = null!;

        public Guid Type { get; set; }

        public string TypeDesc { get; set; } = null!;

        public bool IsPublic { get; set; }

        public string? ShareUrl { get; set; }

        public DateTime ArrivalTime { get; set; }

        public DateTime OpenTime { get; set; }

        public DateTime CloseTime { get; set; }

        public DateTime? CloseRemindTime { get; set; }

        public string? Remark { get; set; }

        public string OrderStatus { get; set; } = null!;

        public string OrderStatusDesc { get; set; } = null!;

        public DateTime CreateTime { get; set; }

        public Guid OfficeID { get; set; }

        public string OfficeName { get; set; } = null!;

        public Guid BrandID { get; set; }

        public string BrandName { get; set; } = null!;

        public string? BrandLogoUrl { get; set; }

        public string? BrandOfficialUrl { get; set; }

        public Guid StoreID { get; set; }

        public string StoreName { get; set; } = null!;

        public string StorePhone { get; set; } = null!;

        public string StoreAddress { get; set; } = null!;
    }

    public class OrderListModel : ViewOrder
    {
        public string BrandStoreName { get; set; }

        public string OfficeOwner { get; set; }

        public string StatusDescPublicDesc { get; set; }

        public string IsPublicDesc { get; set; } = null!;

        public new string ArrivalTime { get; set; }

        public new string CloseTime { get; set; }

        public new string CreateTime { get; set; }

        public bool ShowAdd { get; set; } = false;

        public bool ShowClose { get; set; } = false;

        public bool ShowDelayArrival { get; set; } = false;

        public bool ShowDelayClose { get; set; } = false;

        public bool ShowDelayNotify { get; set; } = false;

        public bool ShowDelayArrivalNotify { get; set; } = false;

        public bool ShowFinish { get; set; } = false;

        public OrderListModel(ViewOrder Entity)
        {
            #region 原始欄位

            OrderID = Entity.OrderID;
            OwnerID = Entity.OwnerID;
            OwnerName = Entity.OwnerName;
            OrderNo = Entity.OrderNo;
            Type = Entity.Type;
            TypeDesc = Entity.TypeDesc;
            IsPublic = Entity.IsPublic;
            ShareUrl = Entity.ShareUrl;
            OpenTime = Entity.OpenTime;
            CloseRemindTime = Entity.CloseRemindTime;
            Remark = Entity.Remark;
            OrderStatus = Entity.OrderStatus;
            OrderStatusDesc = Entity.OrderStatusDesc;
            OfficeID = Entity.OfficeID;
            OfficeName = Entity.OfficeName;
            BrandID = Entity.BrandID;
            BrandName = Entity.BrandName;
            BrandLogoUrl = Entity.BrandLogoUrl;
            BrandOfficialUrl = Entity.BrandOfficialUrl;
            StoreID = Entity.StoreID;
            StoreName = Entity.StoreName;
            StorePhone = Entity.StorePhone;
            StoreAddress = Entity.StoreAddress;

            #endregion

            #region 轉換欄位

            OrderStatusDesc = new OrderExtension().ConvertOrderStatus(Entity.OrderStatus, Entity.CloseTime);
            IsPublicDesc = Entity.IsPublic ? "公團" : "私團";
            ArrivalTime = Entity.ArrivalTime.ToDateHourMinute();
            CloseTime = Entity.CloseTime.ToDateHourMinute();
            CreateTime = Entity.CreateTime.ToDateHourMinute();

            #endregion

            #region 合併欄位

            BrandStoreName = $"{BrandName} {StoreName}";
            OfficeOwner = $"{OfficeName} {OwnerName}";
            StatusDescPublicDesc = $"{OrderStatusDesc} {IsPublicDesc}";

            #endregion
        }

        public OrderListModel SetButton(Guid AccountID)
        {
            switch (OrderStatus)
            {
                case "01":
                    if (DateTime.Now <= Convert.ToDateTime(CloseTime))
                    {
                        ShowAdd = true;
                    }
                    if (OwnerID == AccountID)
                    {
                        ShowAdd = true;
                        ShowClose = true;
                        ShowDelayArrival = true;
                        ShowDelayClose = true;
                        ShowDelayNotify = true;
                        ShowDelayArrivalNotify = true;
                        ShowFinish = true;
                    }
                    break;
                case "02":
                case "98":
                default:
                    break;
            }
            return this;
        }
    }

    #region 訂單明細

    public class ViewOrderDetail
    {
        public Guid OrderDetailID { get; set; }

        public Guid OrderID {  get; set; }

        public Guid? DrinkFoodID { get; set; }

        public string DrinkFoodName { get; set; } = null!;

        public int DrinkFoodPrice { get; set; }

        public string? DrinkFoodRemark { get; set; }

        public Guid? SugarID { get; set; }

        public string SugarDesc { get; set; } = null!;

        public Guid? IceID { get; set; }

        public string IceDesc { get; set; } = null!;

        public Guid? SizeID { get; set; }

        public string SizeDesc { get; set; } = null!;

        public Guid DetailAccountID { get; set; }

        public string Name { get; set; } = null!;

        public string? Brief { get; set; }

        public string Email { get; set; } = null!;

        public Guid? PaymentID { get; set; }

        public string? PaymentDesc { get; set; }

        public DateTime? PaymentDatetime { get; set; }

        public bool? PaymentArrived { get; set; }

        public int? Quantity { get; set; }

        public bool? IsPickup { get; set; }

        public string? DetailRemark { get; set; }

        // 用以判斷刪除按鈕的訂單欄位
        public string OrderStatus {  get; set; } = null!;

        public DateTime CloseTime {  get; set; }

        public Guid OwnerID {  get; set; }
    }


    public class OrderDetailListModel : ViewOrderDetail
    {
        public bool ShowDelete { get; set; } = false;

        public string PickUpDesc { get; set; }

        public OrderDetailListModel(ViewOrderDetail Entity) 
        {
            #region 原始欄位

            OrderID = Entity.OrderID;
            OrderDetailID = Entity.OrderDetailID;
            DrinkFoodID = Entity.DrinkFoodID;
            DrinkFoodName = Entity.DrinkFoodName;
            DrinkFoodPrice = Entity.DrinkFoodPrice;
            DrinkFoodRemark = Entity.DrinkFoodRemark;
            SugarID = Entity.SugarID;
            SugarDesc = Entity.SugarDesc;
            IceID = Entity.IceID;
            IceDesc = Entity.IceDesc;
            SizeID = Entity.SizeID;
            SizeDesc = Entity.SizeDesc;
            DetailAccountID = Entity.DetailAccountID;
            Name = Entity.Name;
            Brief = Entity.Brief;
            Email = Entity.Email;
            PaymentID = Entity.PaymentID;
            PaymentDesc = Entity.PaymentDesc ?? "尚未付款";
            PaymentDatetime = Entity.PaymentDatetime;
            PaymentArrived = Entity.PaymentArrived;
            Quantity = Entity.Quantity;
            IsPickup = Entity.IsPickup;
            PickUpDesc = Entity.IsPickup.HasValue && Entity.IsPickup.Value ? "已取餐" : "尚未取餐";
            DetailRemark = Entity.DetailRemark ?? "無";
            OrderStatus = Entity.OrderStatus;
            CloseTime = Entity.CloseTime;
            OwnerID = Entity.OwnerID;

            #endregion
        }

        public OrderDetailListModel SetButton(Guid AccountID)
        {
            switch (OrderStatus)
            {
                case "01":
                    if (DetailAccountID == AccountID && DateTime.Now <= Convert.ToDateTime(CloseTime))
                    {
                        ShowDelete = true;
                    }
                    if (DetailAccountID == OwnerID)
                    {
                        ShowDelete = true;
                    }
                    break;
                case "02":
                case "98":
                default:
                    break;
            }

            return this;
        }
    }
    

    public class ViewDetailHistory
    {
        public Guid OrderDetailID { get; set; }

        public string ArrivalTime { get; set; }

        public string BrandName { get; set; }

        public string BrandStoreName { get; set; }

        public string DrinkFoodName { get; set; }

        public int DrinkFoodPrice { get; set; }

        public string? DetailRemark { get; set; }

        public string? PaymentDesc { get; set; }

        public int? Quantity { get; set; }

        public string OfficeName { get; set; }

        public Guid DetailAccountID { get; set; }

        public ViewDetailHistory(OrderDetailListModel OrderDetail, OrderListModel listModel)
        {
            OrderDetailID = OrderDetail.OrderDetailID;
            ArrivalTime = listModel.ArrivalTime;
            BrandName = listModel.BrandName;
            BrandStoreName = listModel.BrandStoreName;
            DrinkFoodName = $"{OrderDetail.DrinkFoodName} / {OrderDetail.SugarDesc} / {OrderDetail.IceDesc}";
            DrinkFoodPrice = OrderDetail.DrinkFoodPrice;
            DetailRemark = OrderDetail.DetailRemark;
            PaymentDesc = OrderDetail.PaymentDesc;
            Quantity = OrderDetail.Quantity;
            OfficeName = listModel.OfficeName;
            DetailAccountID = OrderDetail.DetailAccountID;
        }
    }

    public class RequestPostOrderDetailModel : PostOrderDetailModel
    {

    }

    public class PostOrderDetailModel
    {
        public Guid OD_order_id { get; set; }

        public Guid OD_drink_food_id { get; set; }

        public Guid OD_sugar_id { get; set; }

        public Guid OD_ice_id { get; set; }

        public Guid OD_size_id { get; set; }

        public Guid OD_account_id { get; set; }

        public string? OD_remark { get; set; }
    }

    public class GroupOrderDetailModel
    {
        public string Name { get; set; } = null!;

        public int TotalPrice { get; set; }

        public int TotalQuantity { get; set; }

        public List<OrderDetailListModel> OrderDetailList { get; set; } = new();
    }

    public class RequestPutPaymentModel
    {
        public Guid? PaymentID { get; set; }
    }

    public class RequestPutPaymentDateTimeModel
    {
        public DateTime? PaymentDateTime { get; set; }
    }

    #endregion

    public class ViewOrderAndDetail
    {
        public string? BrandOfficialUrl { get; set; }

        public string? BrandLogoUrl { get; set; }

        public string BrandStoreName { get; set; }

        public string StorePhone { get; set; }

        public string StoreAddress { get; set; }

        public Guid OrderID { get; set; }

        public string OwnerName { get; set; }

        public string OrderNo { get; set; }

        public string OrderStatusDesc { get; set; }

        public string OfficeName { get; set; }

        public string ArrivalTime { get; set; }

        public string CloseTime { get; set; }

        public string CreateTime { get; set; }

        public bool ShowAdd { get; set; } = false;

        public bool ShowClose { get; set; } = false;

        public bool ShowDelayArrival { get; set; } = false;

        public bool ShowDelayClose { get; set; } = false;

        public bool ShowDelayNotify {  get; set; } = false;

        public bool ShowDelayArrivalNotify { get; set; } = false;

        public bool ShowFinish { get; set; } = false;

        public List<GroupOrderDetailModel> Detail { get; set; }

        public ViewOrderAndDetail(OrderListModel Entity, List<GroupOrderDetailModel> EntityData)
        {
            BrandOfficialUrl = Entity.BrandOfficialUrl;
            BrandLogoUrl = Entity.BrandLogoUrl;
            BrandStoreName = Entity.BrandStoreName;
            StorePhone = Entity.StorePhone;
            StoreAddress = Entity.StoreAddress;
            OrderID = Entity.OrderID;
            OrderNo = Entity.OrderNo;
            OrderStatusDesc = Entity.OrderStatusDesc;
            ArrivalTime = Entity.ArrivalTime;
            CloseTime = Entity.CloseTime;
            OfficeName = Entity.OfficeName;
            OwnerName = Entity.OwnerName;
            CreateTime = Entity.CreateTime;
            ShowAdd = Entity.ShowAdd;
            ShowClose = Entity.ShowClose;
            ShowDelayArrival = Entity.ShowDelayArrival;
            ShowDelayClose = Entity.ShowDelayClose;
            ShowDelayNotify = Entity.ShowDelayNotify;
            ShowDelayArrivalNotify = Entity.ShowDelayArrivalNotify;
            ShowFinish = Entity.ShowFinish;
            Detail = EntityData;
        }
    }

    #region 狀態轉換


    public class OrderExtension
    {
        public string ConvertOrderStatus(string OrderStatus, DateTime CloseTime)
        {
            return OrderStatus switch
            {
                "99" => "刪除",
                "98" => "關閉",
                "01" => DateTime.Now > CloseTime ? "已結單" : "開放點餐",
                _ => "",
            };
        }
    }


    #endregion
}
