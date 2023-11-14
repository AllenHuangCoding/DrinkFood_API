namespace DrinkFood_API.Models
{
    public class RequestGetMyOrderListModel
    {
    }

    public class RequestPostOrderModel
    {
        public Guid OfficeID { get; set; }

        public Guid CreateAccountID { get; set; }

        public Guid StoreID  { get; set; }

        public Guid OrderTypeID { get; set; }

        public DateTime DrinkTime { get; set; }

        public DateTime CloseTime { get; set; }

    }

    public class RequestPutOrderTimeModel
    {
        public DateTime DrinkTime { get; set; }

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

        public string SetOrderStatus(string Status)
        {
            return Status switch
            {
                "99" => "刪除",
                "98" => "關閉",
                _ => "",
            };
        }
    }

    public class OrderListModel : ViewOrder
    {
        public string BrandStoreName { get; set; }

        public new string ArrivalTime { get; set; }

        public new string CloseTime { get; set; }

        public new string CreateTime { get; set; }

        public OrderListModel(ViewOrder Entity)
        {
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

            BrandStoreName = $"{BrandName} {StoreName}";
            ArrivalTime = Entity.ArrivalTime.ToString("yyyy-MM-dd HH:mm");
            CloseTime = Entity.CloseTime.ToString("yyyy-MM-dd HH:mm");
            CreateTime = Entity.CreateTime.ToString("yyyy-MM-dd HH:mm");
        }
    }

    #region 訂單明細

    public class ViewOrderDetail
    {
        public Guid OrderDetailID { get; set; }

        public Guid OrderID {  get; set; }

        public Guid DrinkFoodID { get; set; }

        public string DrinkFoodName { get; set; } = null!;

        public int DrinkFoodPrice { get; set; }

        public string? DrinkFoodRemark { get; set; }

        public Guid SugarID { get; set; }

        public string SugarDesc { get; set; } = null!;

        public Guid IceID { get; set; }

        public string IceDesc { get; set; } = null!;

        public Guid SizeID { get; set; }

        public string SizeDesc { get; set; } = null!;

        public Guid AccountID { get; set; }

        public string Name { get; set; } = null!;

        public string? Brief { get; set; }

        public string Email { get; set; } = null!;

        public Guid? PaymentID { get; set; }

        public string? PaymentDesc { get; set; }

        public DateTime? PaymentDatetime { get; set; }

        public bool? PaymentArrived { get; set; }

        public int Quantity { get; set; }

        public bool IsPickup { get; set; }

        public string? DetailRemark { get; set; }
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

        public int Quantity { get; set; }

        public string OfficeName { get; set; }


        public ViewDetailHistory(ViewOrderDetail OrderDetail, ViewOrder Order)
        {
            OrderListModel listModel = new(Order);

            OrderDetailID = OrderDetail.OrderDetailID;
            ArrivalTime = listModel.ArrivalTime;
            BrandName = Order.BrandName;
            BrandStoreName = listModel.BrandStoreName;
            DrinkFoodName = $"{OrderDetail.DrinkFoodName} / {OrderDetail.SugarDesc} / {OrderDetail.IceDesc}";
            DrinkFoodPrice = OrderDetail.DrinkFoodPrice;
            DetailRemark = OrderDetail.DetailRemark;
            PaymentDesc = OrderDetail.PaymentDesc;
            Quantity = OrderDetail.Quantity;
            OfficeName = Order.OfficeName;
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

        public List<ViewOrderDetail> OrderDetailList { get; set; } = new();
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


    public class ViewOrderAndDetail : OrderListModel
    {
        public List<GroupOrderDetailModel> Detail { get; set; }

        public ViewOrderAndDetail(ViewOrder Entity, List<GroupOrderDetailModel> EntityData) : base(Entity)
        {
            Detail = EntityData;
        }
    }
}
