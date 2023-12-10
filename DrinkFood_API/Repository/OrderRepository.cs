﻿using CodeShare.Libs.GenericEntityFramework;
using DataBase.Entities;
using DataBase;
using DrinkFood_API.Models;
using CodeShare.Libs.BaseProject;

namespace DrinkFood_API.Repository
{
    public class OrderRepository : BaseTable<EFContext, Order>
    {
        /// <summary>
        /// 建構元 
        /// </summary>
        public OrderRepository(IServiceProvider provider) : base(provider)
        {
            
        }

        #region 訂單查詢 (View)

        public IQueryable<ViewOrder> GetViewOrder()
        {
            return from order in _readDBContext.Order
                   join store in _readDBContext.Store on order.O_store_id equals store.S_id
                   join brand in _readDBContext.Brand on store.S_brand_id equals brand.B_id
                   join account in _readDBContext.Account on order.O_create_account_id equals account.A_id
                   join office in _readDBContext.Office on order.O_office_id equals office.O_id
                   join typeCodeTable in _readDBContext.CodeTable.Where(x => x.CT_type == "OrderType") on order.O_type equals typeCodeTable.CT_id
                   where order.O_status != "99" && store.S_status != "99" && account.A_status != "99" && office.O_status != "99"
                   select new ViewOrder
                   {
                       OrderID = order.O_id,
                       OwnerID = order.O_create_account_id,
                       OwnerName = account.A_name,
                       OwnerBrief = account.A_brief,
                       OrderNo = order.O_no,
                       Type = order.O_type,
                       TypeDesc = typeCodeTable.CT_desc,
                       IsPublic = order.O_is_public,
                       ShareUrl = order.O_share_url,
                       ArrivalTime = order.O_arrival_time,
                       OpenTime = order.O_open_time,
                       CloseTime = order.O_close_time,
                       CloseRemindTime = order.O_close_remind_time,
                       Remark = order.O_remark,
                       OrderStatus = order.O_status,
                       OrderStatusDesc = "尚未設定",
                       CreateTime = order.O_create,
                       OfficeID = order.O_office_id,
                       OfficeName = office.O_name,
                       BrandID = brand.B_id,
                       BrandName = brand.B_name,
                       BrandLogoUrl = brand.B_logo,
                       BrandOfficialUrl = brand.B_official_url,
                       StoreID = order.O_store_id,
                       StoreName = store.S_name,
                       StoreAddress = store.S_address,
                       StorePhone = store.S_phone,
                   };
        }

        public OrderListModel GetOrderListData(Guid OrderID)
        {
            ViewOrder? viewOrder = GetViewOrder().Where(x =>
                x.OrderID == OrderID
            ).FirstOrDefault() ?? throw new ApiException("訂單ID不存在", 400);

            return new OrderListModel(viewOrder);
        }

        #endregion

        #region 更改訂單欄位 (用餐時間/結單時間)

        /// <summary>
        /// 更改用餐時間
        /// </summary>
        /// <param name="OrderID"></param>
        /// <param name="time"></param>
        /// <exception cref="ApiException"></exception>
        public void PutArrivalTime(Guid OrderID, DateTime time)
        {
            var order = GetById(OrderID) ?? throw new ApiException("訂單ID不存在", 400);
            order.O_arrival_time = time;
            order.O_update = DateTime.Now;
            Update(OrderID, order);
        }

        /// <summary>
        /// 更改結單時間
        /// </summary>
        /// <param name="OrderID"></param>
        /// <param name="time"></param>
        /// <exception cref="ApiException"></exception>
        public void PutCloseTime(Guid OrderID, DateTime time)
        {
            var order = GetById(OrderID) ?? throw new ApiException("訂單ID不存在", 400);
            order.O_close_time = time;
            order.O_update = DateTime.Now;
            Update(OrderID, order);
        }

        #endregion

        #region 更改訂單狀態 (關閉訂單/完成訂單)

        /// <summary>
        /// 關閉訂單 (限團長與本人)
        /// </summary>
        /// <param name="OrderID"></param>
        /// <exception cref="ApiException"></exception>
        public void CloseOrder(Guid OrderID)
        {
            var order = GetById(OrderID) ?? throw new ApiException("訂單ID不存在", 400);
            order.O_status = "98";
            order.O_update = DateTime.Now;
            Update(OrderID, order);
        }

        /// <summary>
        /// 完成訂單 (限團長)
        /// </summary>
        /// <param name="OrderID"></param>
        /// <exception cref="ApiException"></exception>
        public void FinishOrder(Guid OrderID)
        {
            var order = GetById(OrderID) ?? throw new ApiException("訂單ID不存在", 400);
            order.O_status = "02";
            order.O_update = DateTime.Now;
            Update(OrderID, order);
        }

        #endregion

        #region 其他方法 (檢查訂單團長)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AccountID"></param>
        /// <param name="OrderID"></param>
        /// <exception cref="ApiException"></exception>
        public void CheckOwnerOrder(Guid AccountID, Guid OrderID)
        {
            var order = GetViewOrder().Where(x =>
                x.OrderID == OrderID
            ).FirstOrDefault() ?? throw new ApiException("訂單不存在", 400);

            if (order.OwnerID != AccountID)
            {
                throw new ApiException("非團長權限", 400);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Entity"></param>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        public bool IsOwnerOrder(ViewOrder Entity, Guid AccountID)
        {
            if (Entity.OwnerID == AccountID)
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}
