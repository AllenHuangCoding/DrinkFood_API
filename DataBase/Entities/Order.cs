using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataBase.Entities;

[Table("Order")]
public partial class Order
{
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid O_id { get; set; }

    public Guid O_office_id { get; set; }

    public Guid O_create_account_id { get; set; }

    public Guid O_store_id { get; set; }

    public string O_no { get; set; }

    public Guid O_type { get; set; }

    public bool O_is_public {  get; set; }

    public string? O_share_url { get; set; }

    public DateTime O_arrival_time { get; set; }

    public DateTime O_open_time { get; set; }

    public DateTime O_close_time { get; set; }

    public DateTime? O_close_remind_time { get; set; }

    public string? O_remark { get; set; }

    /// <summary>
    /// <para>01: 正常 (開放點餐/已結單)</para>
    /// <para>02: 已完成</para>
    /// <para>98: 關閉</para>
    /// <para>99: 刪除</para>
    /// </summary>
    public string O_status { get; set; }

    public DateTime O_create { get; set; }

    public DateTime O_update { get; set; }

    public Order()
    {
        O_id = Guid.NewGuid();
        O_no = "";
        O_status = "01";
        O_create = DateTime.Now;
        O_update = DateTime.Now;
    }
}