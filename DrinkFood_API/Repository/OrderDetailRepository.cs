using CodeShare.Libs.GenericEntityFramework;
using DataBase.Entities;
using DataBase;
using DrinkFood_API.Models;
using CodeShare.Libs.BaseProject;

namespace DrinkFood_API.Repository
{
    public class OrderDetailRepository : BaseTable<EFContext, OrderDetail>
    {
        [Inject] private readonly OrderRepository _orderRepository;

        /// <summary>
        /// 建構元 
        /// </summary>
        public OrderDetailRepository(IServiceProvider provider) : base(provider)
        {
            provider.Inject(this);
        }

        public void CheckMyOrderDetail(Guid AccountID, Guid OrderDetailID)
        {
            var orderDetail = GetViewOrderDetail().Where(x =>
                x.OrderDetailID == OrderDetailID
            ).FirstOrDefault() ?? throw new ApiException("訂單內容不存在", 400);

            if (orderDetail.AccountID != AccountID)
            {
                throw new ApiException("非點餐者權限", 400);
            }
        }

        public List<GroupOrderDetailModel> GroupOrderDetailByName(List<ViewOrderDetail> Data)
        {
            return Data.GroupBy(x => 
                x.Name
            ).Select(x =>
                new GroupOrderDetailModel
                {
                    Name = x.Key,
                    OrderDetailList = x.ToList()
                }
            ).ToList();
        }

        public List<ViewDetailHistory> CombineDetailHistory(List<ViewOrderDetail> OrderDetail, List<ViewOrder> Order)
        {
            return OrderDetail.Select(x => 
                new ViewDetailHistory(
                    x,
                    Order.First(o => o.OrderID == x.OrderID)
                )
            ).ToList();
        }

        public void PostOrderDetail(PostOrderDetailModel Data)
        {
            Create(new OrderDetail
            {
                OD_order_id = Data.OD_order_id,
                OD_drink_food_id = Data.OD_drink_food_id,
                OD_sugar_id = Data.OD_sugar_id,
                OD_ice_id = Data.OD_ice_id,
                OD_size_id = Data.OD_size_id,
                OD_account_id = Data.OD_account_id,
                OD_remark = Data.OD_remark,
            });
        }

        public void DeleteOrderDetail(Guid OrderDetailID)
        {
            _ = GetViewOrderDetail().Where(x =>
                x.OrderDetailID == OrderDetailID
            ).FirstOrDefault() ?? throw new ApiException("訂單內容不存在", 400);

            Delete(OrderDetailID);
        }

        public void PutPayment(Guid OrderDetailID, Guid? PaymentID)
        {
            var orderDetail = GetById(OrderDetailID) ?? throw new ApiException("訂單內容不存在", 400);
            orderDetail.OD_payment_id = PaymentID;
            orderDetail.OD_update = DateTime.Now;
            Update(OrderDetailID, orderDetail);
        }

        public void PutPaymentDatetime(Guid OrderDetailID, DateTime? PaymentDateTime)
        {
            var orderDetail = GetById(OrderDetailID) ?? throw new ApiException("訂單內容不存在", 400);
            orderDetail.OD_payment_datetime = PaymentDateTime;
            orderDetail.OD_update = DateTime.Now;
            Update(OrderDetailID, orderDetail);
        }

        public IQueryable<ViewOrderDetail> GetViewOrderDetail()
        {
            return from orderDetail in _readDBContext.OrderDetail
                   join order in _readDBContext.Order on orderDetail.OD_order_id equals order.O_id
                   join drinkFood in _readDBContext.DrinkFood on orderDetail.OD_drink_food_id equals drinkFood.DF_id
                   join account in _readDBContext.Account on orderDetail.OD_account_id equals account.A_id
                   join sugarOption in _readDBContext.Option on orderDetail.OD_sugar_id equals sugarOption.O_id
                   join iceOption in _readDBContext.Option on orderDetail.OD_ice_id equals iceOption.O_id
                   join sizeOption in _readDBContext.Option on orderDetail.OD_size_id equals sizeOption.O_id
                   join paymentCodeTable in _readDBContext.CodeTable.Where(x => x.CT_type == "Payment") on orderDetail.OD_payment_id equals paymentCodeTable.CT_id
                   join office in _readDBContext.Office on order.O_office_id equals office.O_id
                   join store in _readDBContext.Store on order.O_store_id equals store.S_id
                   join brand in _readDBContext.Brand on store.S_brand_id equals brand.B_id
                   select new ViewOrderDetail
                   {
                       OrderID = orderDetail.OD_order_id,
                       OrderDetailID = orderDetail.OD_id,
                       DrinkFoodID = orderDetail.OD_drink_food_id,
                       DrinkFoodName = drinkFood.DF_name,
                       DrinkFoodPrice = drinkFood.DF_price,
                       DrinkFoodRemark = drinkFood.DF_remark ?? "",
                       SugarID = orderDetail.OD_sugar_id,
                       SugarDesc = sugarOption.O_name,
                       IceID = orderDetail.OD_ice_id,
                       IceDesc = iceOption.O_name,
                       SizeID = orderDetail.OD_size_id,
                       SizeDesc = sizeOption.O_name,
                       AccountID = orderDetail.OD_account_id,
                       Name = account.A_name,
                       Brief = account.A_brief,
                       Email = account.A_email,
                       PaymentID = orderDetail.OD_payment_id,
                       PaymentDesc = paymentCodeTable.CT_desc,
                       PaymentDatetime = orderDetail.OD_payment_datetime,
                       PaymentArrived = orderDetail.OD_payment_datetime.HasValue,
                       Quantity = orderDetail.OD_quantity,
                       IsPickup = orderDetail.OD_pickup,
                       DetailRemark = orderDetail.OD_remark,

                       //OrderID = orderDetail.OD_order_id,
                       //ArrivalTime = order.O_arrival_time,
                       //OrderStatus = order.O_status,
                       //OrderStatusDesc = "尚未設定",
                       //OwnerID = order.O_create_account_id,
                       //OfficeID = order.O_office_id,
                       //OfficeName = office.O_name,
                       //StoreID = order.O_store_id,
                       //StoreName = store.S_name,
                       //BrandID = brand.B_id,
                       //BrandName = brand.B_name,
                   };
        }

        public OrderDetail? Exist(Guid OrderDetailID)
        {
            var orderDetail = FindOne(x =>
                x.OD_id == OrderDetailID && x.OD_status != "99"
            );

            if (orderDetail == null)
            {
                return null;
            }
            return orderDetail;
        }
    }
}
