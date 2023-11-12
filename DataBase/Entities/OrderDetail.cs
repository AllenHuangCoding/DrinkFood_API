using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataBase.Entities;

[Table("OrderDetail")]
public partial class OrderDetail
{
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid OD_id { get; set; }

    public Guid OD_order_id { get; set; }

    public Guid OD_drink_food_id { get; set; }

    public Guid OD_sugar_id { get; set; }

    public Guid OD_ice_id { get; set; }

    public Guid OD_size_id { get; set; }

    public Guid OD_account_id { get; set; }

    public Guid OD_create_account_id { get; set; }

    public Guid? OD_payment_id { get; set; }

    public DateTime? OD_payment_datetime { get; set; }

    public int OD_quantity { get; set; }

    public bool OD_pickup { get; set; }

    public string? OD_remark { get; set; }

    public string OD_status {  get; set; }

    public DateTime OD_create { get; set; }

    public DateTime OD_update { get; set; }

    public OrderDetail() 
    { 
        OD_id = Guid.NewGuid();
        OD_status = "01";
        OD_create = DateTime.Now;
        OD_update = DateTime.Now;
    }
}
