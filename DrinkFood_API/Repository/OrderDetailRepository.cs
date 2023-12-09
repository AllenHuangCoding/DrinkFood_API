﻿using CodeShare.Libs.GenericEntityFramework;
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

        #region 訂單明細查詢 (View)

        public IQueryable<ViewOrderDetail> GetViewOrderDetail()
        {
            return from orderDetail in _readDBContext.OrderDetail
                   join order in _readDBContext.Order on orderDetail.OD_order_id equals order.O_id
                   join drinkFood in _readDBContext.DrinkFood on orderDetail.OD_drink_food_id equals drinkFood.DF_id
                   join account in _readDBContext.Account on orderDetail.OD_account_id equals account.A_id
                   join sugarOption in _readDBContext.Option on orderDetail.OD_sugar_id equals sugarOption.O_id
                   join iceOption in _readDBContext.Option on orderDetail.OD_ice_id equals iceOption.O_id
                   join sizeOption in _readDBContext.Option on orderDetail.OD_size_id equals sizeOption.O_id
                   join paymentCodeTable in _readDBContext.CodeTable.Where(x => x.CT_type.Contains("Payment")) on orderDetail.OD_payment_id equals paymentCodeTable.CT_id
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
                       DetailAccountID = orderDetail.OD_account_id,
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
                       OrderStatus = order.O_status,
                       CloseTime = order.O_close_time,
                       OwnerID = order.O_create_account_id
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

        #endregion

        #region 訂單明細流程動作 (加入品項/刪除品項)

        /// <summary>
        /// 加入品項 (限本人)
        /// </summary>
        /// <param name="Data"></param>
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

        /// <summary>
        /// 刪除品項 (限團長與本人)
        /// </summary>
        /// <param name="OrderDetailID"></param>
        /// <exception cref="ApiException"></exception>
        public void DeleteOrderDetail(Guid OrderDetailID)
        {
            _ = GetViewOrderDetail().Where(x =>
                x.OrderDetailID == OrderDetailID
            ).FirstOrDefault() ?? throw new ApiException("訂單內容不存在", 400);

            Delete(OrderDetailID);
        }

        #endregion

        #region 更改訂單明細欄位 (更改付款方式/更改付款時間/更改取餐狀態)

        /// <summary>
        /// 更改付款方式 (限團長)
        /// </summary>
        /// <param name="OrderDetailID"></param>
        /// <param name="PaymentID"></param>
        /// <exception cref="ApiException"></exception>
        public void PutPayment(Guid OrderDetailID, Guid? PaymentID)
        {
            var orderDetail = GetById(OrderDetailID) ?? throw new ApiException("訂單內容不存在", 400);
            orderDetail.OD_payment_id = PaymentID;
            orderDetail.OD_update = DateTime.Now;
            Update(OrderDetailID, orderDetail);
        }

        /// <summary>
        /// 更改付款時間 (限團長)
        /// </summary>
        /// <param name="OrderDetailID"></param>
        /// <param name="PaymentDateTime"></param>
        /// <exception cref="ApiException"></exception>
        public void PutPaymentDatetime(Guid OrderDetailID, DateTime? PaymentDateTime)
        {
            var orderDetail = GetById(OrderDetailID) ?? throw new ApiException("訂單內容不存在", 400);
            orderDetail.OD_payment_datetime = PaymentDateTime;
            orderDetail.OD_update = DateTime.Now;
            Update(OrderDetailID, orderDetail);
        }

        /// <summary>
        /// 更改取餐狀態 (限團長)
        /// </summary>
        public void PutPickup()
        {

        }

        #endregion

        #region 其他方法 (訂單明細依訂購者分群)

        /// <summary>
        /// 訂單明細 (依訂購者分群)
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public List<GroupOrderDetailModel> GroupOrderDetailByName(List<OrderDetailListModel> Data)
        {
            return Data.GroupBy(x => new {
                x.Email,
                x.Name,
                x.Brief
            }).Select(x =>
                new GroupOrderDetailModel
                {
                    Name = !string.IsNullOrWhiteSpace(x.Key.Brief) ? x.Key.Brief : x.Key.Name,
                    TotalPrice = x.Select(x => x.DrinkFoodPrice * x.Quantity.Value).Sum(),
                    TotalQuantity = x.Where(x => x.Quantity.HasValue).Select(x => x.Quantity.Value).Sum(),
                    OrderDetailList = x.ToList(),
                }
            ).ToList();
        }

        public List<ViewDetailHistory> CombineDetailHistory(List<OrderDetailListModel> OrderDetail, List<OrderListModel> Order)
        {
            return OrderDetail.Select(x =>
                new ViewDetailHistory(
                    x,
                    Order.First(o => o.OrderID == x.OrderID)
                )
            ).ToList();
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

        public void CheckMyOrderDetail(Guid AccountID, Guid OrderDetailID)
        {
            var orderDetail = GetViewOrderDetail().Where(x =>
                x.OrderDetailID == OrderDetailID
            ).FirstOrDefault() ?? throw new ApiException("訂單內容不存在", 400);

            if (orderDetail.DetailAccountID != AccountID)
            {
                throw new ApiException("非點餐者權限", 400);
            }
        }

        #endregion
    }
}
