using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataBase.View
{
    public class ViewOrderDetail
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid OrderDetailID { get; set; }

        public Guid OrderID { get; set; }

        public Guid? DrinkFoodID { get; set; }

        public string? DrinkFoodName { get; set; }

        public int? DrinkFoodPrice { get; set; }

        public string? DrinkFoodRemark { get; set; }

        public Guid? SugarID { get; set; }

        public string? SugarDesc { get; set; }

        public Guid? IceID { get; set; }

        public string? IceDesc { get; set; }

        public Guid? SizeID { get; set; }

        public string? SizeDesc { get; set; }

        public Guid DetailAccountID { get; set; }

        public string Name { get; set; } = null!;

        public string? Brief { get; set; }

        public string Email { get; set; } = null!;

        public Guid? PaymentID { get; set; }

        public string? PaymentDesc { get; set; }

        public DateTime? PaymentDatetime { get; set; }

        public int? Quantity { get; set; }

        public bool? IsPickup { get; set; }

        public string? DetailRemark { get; set; }

        // 用以判斷刪除按鈕的訂單欄位
        public string OrderStatus { get; set; } = null!;

        public DateTime ArrivalTime { get; set; }

        public DateTime CloseTime { get; set; }

        public Guid OwnerID { get; set; }

        public string BrandName { get; set; } = null!;
    }
}
