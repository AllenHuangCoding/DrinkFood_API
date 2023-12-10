using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataBase.View
{
    public class ViewOrder
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid OrderID { get; set; }

        public Guid OwnerID { get; set; }

        public string OwnerName { get; set; } = null!;

        public string? OwnerBrief { get; set; }

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
    }
}
