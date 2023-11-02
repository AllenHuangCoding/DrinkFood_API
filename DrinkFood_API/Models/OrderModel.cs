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

        public string OrderStatus { get; set; } = null!;

        public string OrderStatusDesc { get; set; } = null!;

        public Guid OfficeID { get; set; }

        public string OfficeName { get; set; } = null!;

        public Guid CreateAccountID { get; set; }

        public string CreateName { get; set; } = null!;

        public Guid StoreID { get; set; }

        public string StoreName { get; set; } = null!;

        public string OrderNo { get; set; } = null!;

        public string? OrderShareUrl { get; set; }

        public string DrinkTime { get; set; } = null!;

        public string CloseTime { get; set; } = null!;

        public string CreateTime { get; set; } = null!;

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

    #region 訂單明細

    public class ViewOrderDetail
    {
        public Guid OrderDetailID { get; set; }

        public Guid OrderID { get; set; }

        public Guid OrderCreateAccountID { get; set; }

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

        public string? Remark { get; set; }
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


    public class ViewOrderAndDetail : ViewOrder
    {
        public List<GroupOrderDetailModel> Detail { get; set; }

        public ViewOrderAndDetail(ViewOrder Entity, List<GroupOrderDetailModel> EntityData) 
        { 
            OrderID = Entity.OrderID;
            OrderStatus = Entity.OrderStatus;
            OrderStatusDesc = Entity.OrderStatusDesc;
            OfficeID = Entity.OfficeID;
            OfficeName = Entity.OfficeName;
            CreateAccountID = Entity.CreateAccountID;
            CreateName = Entity.CreateName;
            StoreID = Entity.StoreID;
            StoreName = Entity.StoreName;
            OrderID = Entity.OrderID;
            OrderNo = Entity.OrderNo;
            OrderShareUrl = Entity.OrderShareUrl;
            DrinkTime = Entity.DrinkTime;
            CloseTime = Entity.CloseTime;

            Detail = EntityData;
        }
    }
}
