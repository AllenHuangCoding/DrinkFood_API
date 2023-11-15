using CodeShare.Libs.Excel;
using DrinkFood_API.Models;
using DrinkFood_API.Repository;
using DrinkFood_API.Service;
using Microsoft.AspNetCore.Mvc;

namespace DrinkFood_API.Services
{
    public class ExportService : BaseService
    {
        [Inject] private readonly OrderDetailRepository _orderDetailRepository;

        [Inject] private readonly OrderRepository _orderRepository;

        public ExportService(IServiceProvider provider) : base()
        {
            provider.Inject(this);
        }

        public FileResult ExportOrderDetailHistory(Guid AccountID)
        {
            var orderDetail = _orderDetailRepository.GetViewOrderDetail().Where(x =>
                x.AccountID == AccountID
            ).ToList();

            var orderIDs = orderDetail.Select(x => x.OrderID).ToList();

            var order = _orderRepository.GetViewOrder().Where(x => orderIDs.Contains(x.OrderID)).ToList();

            var history = orderDetail.Select(x =>
                new DetailHistoryExcelModel(
                    x,
                    order.First(o => o.OrderID == x.OrderID)
                )
            ).ToList();

            return new ExportExcel("./Template", "History.xlsx").DataToTableContent(history).ToExcel("消費紀錄");
        }

        public List<int> ExportMonthReport(RequestMonthReportModel RequestData)
        {
            return new List<int>();
        }
    }
}
