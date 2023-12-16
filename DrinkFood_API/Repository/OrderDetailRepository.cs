using CodeShare.Libs.GenericEntityFramework;
using DataBase.Entities;
using DataBase;
using DrinkFood_API.Models;
using CodeShare.Libs.BaseProject;
using DataBase.View;

namespace DrinkFood_API.Repository
{
    public class ViewOrderDetailRepository : BaseView<EFContext, ViewOrderDetail>
    {
        public ViewOrderDetailRepository(IServiceProvider provider) : base(provider)
        {
            provider.Inject(this);
        }
    }

    public class OrderDetailRepository : BaseTable<EFContext, OrderDetail>
    {
        [Inject] private readonly OrderRepository _orderRepository;

        [Inject] private readonly ViewOrderDetailRepository _viewOrderDetailRepository;



        /// <summary>
        /// 建構元 
        /// </summary>
        public OrderDetailRepository(IServiceProvider provider) : base(provider)
        {
            provider.Inject(this);
        }

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
            var orderDetail = GetById(OrderDetailID) ?? throw new ApiException("訂單內容不存在", 400);
            orderDetail.OD_status = "99";
            orderDetail.OD_update = DateTime.Now;
            Update(OrderDetailID, orderDetail);
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
            orderDetail.OD_payment_datetime = PaymentDateTime.HasValue ? PaymentDateTime.Value.ToLocalTime() : null;
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
                    TotalPrice = x.Where(x => x.DrinkFoodPrice.HasValue && x.Quantity.HasValue).Select(x => x.DrinkFoodPrice.Value * x.Quantity.Value).Sum(),
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
            var orderDetail = _viewOrderDetailRepository.FindAll(x =>
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
