using DataBase.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace DrinkFood_API.Models
{
    public class RequestMonthReportModel
    {
        public DateTime Month { get; set; }

        public List<Guid>? PaymentID { get; set; }
    }

    public class DetailHistoryExcelModel
    {
        [Column(Order = 0)]
        public string ArrivalTime { get; set; }

        [Column(Order = 1)]
        public string BrandName { get; set; }

        [Column(Order = 2)]
        public string StoreName { get; set; }

        [Column(Order = 3)]
        public string DrinkFoodName { get; set; }

        [Column(Order = 4)]
        public string SugarDesc { get; set; }

        [Column(Order = 5)]
        public string IceDesc { get; set; }

        [Column(Order = 6)]
        public int DrinkFoodPrice { get; set; }

        [Column(Order = 7)]
        public int? Quantity { get; set; }

        [Column(Order = 8)]
        public string? DetailRemark { get; set; }

        [Column(Order = 9)]
        public string? PaymentDesc { get; set; }

        [Column(Order = 10)]
        public string OfficeName { get; set; }

        public DetailHistoryExcelModel(ViewOrderDetail OrderDetail, ViewOrder Order)
        {
            OrderListModel listModel = new(Order, Guid.NewGuid());
            ArrivalTime = listModel.ArrivalTime;
            BrandName = Order.BrandName;
            StoreName = listModel.StoreName;
            DrinkFoodName = OrderDetail.DrinkFoodName;
            SugarDesc = OrderDetail.SugarDesc;
            IceDesc = OrderDetail.IceDesc;
            DrinkFoodPrice = OrderDetail.DrinkFoodPrice;
            Quantity = OrderDetail.Quantity;
            DetailRemark = OrderDetail.DetailRemark;
            PaymentDesc = OrderDetail.PaymentDesc;
            OfficeName = Order.OfficeName;
        }
    }

    public class MonthExportExcelModel
    {
        public string Brief { get; set; }


        public MonthExportExcelModel(string Key) 
        { 
            Brief = Key;
        }
    }
}
