﻿using CodeShare.Libs.Excel;
using CodeShare.Libs.BaseProject;
using DrinkFood_API.Models;
using DrinkFood_API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DrinkFood_API.Services
{
    public class ExportService : BaseService
    {
        [Inject] private readonly OrderDetailRepository _orderDetailRepository;

        [Inject] private readonly OrderRepository _orderRepository;

        [Inject] private readonly ViewOrderRepository _viewOrderRepository;

        [Inject] private readonly ViewOrderDetailRepository _viewOrderDetailRepository;

        public ExportService(IServiceProvider provider) : base(provider)
        {
            provider.Inject(this);
        }

        public FileResult ExportOrderDetailHistory(Guid AccountID)
        {
            var orderDetail = _viewOrderDetailRepository.GetAll().Where(x =>
                x.DetailAccountID == AccountID
            ).ToList();

            var orderIDs = orderDetail.Select(x => x.OrderID).ToList();

            var order = _viewOrderRepository.GetAll().Where(x => orderIDs.Contains(x.OrderID)).ToList();

            var history = orderDetail.Select(x =>
                new DetailHistoryExcelModel(
                    x,
                    order.First(o => o.OrderID == x.OrderID)
                )
            ).ToList();

            return new ExportExcel("./Template", "History.xlsx").DataToTableContent(history).ToExcel("消費紀錄");
        }

        public FileResult ExportMonthReport(RequestMonthReportModel RequestData)
        {
            var sd = RequestData.Month;
            var ed = RequestData.Month.AddMonths(1);

            var order = _orderRepository.FindAll(x =>
                sd <= x.O_arrival_time && x.O_arrival_time < ed
            ).ToList();

            var orderIDs = order.Select(x => x.O_id).ToList();
            var orderDetail = _viewOrderDetailRepository.GetAll().Where(x =>
                orderIDs.Contains(x.OrderID)
            ).ToList();

            var monthData = orderDetail.GroupBy(x => 
                new { x.Name }
            ).Select(x => 
                new MonthExportExcelModel(x.Key.Name)
            ).ToList();

            // 如何將不固定Title的寫成套件轉Excel?
            var excel = new ExportExcel("./Template", "EmptyMonth.xlsx");
            //excel.SetHeader(Month.GenerateDatesInMonth(sd.Year, sd.Month), 0, 0, 1);
            //excel.SetHeader(Month.GetDaysOfWeekInMonth(sd.Year, sd.Month), 0, 1, 1);

            //List<Action> list = new List<Action>()
            //{
            //    ()=>{},
            //    ()=>{}
            //};

            return excel.ToExcel("扣款表");
        }
    }
}
