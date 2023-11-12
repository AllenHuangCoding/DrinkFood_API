using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataBase.Entities;

[Table("Account")]
public partial class Account
{
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid A_id { get; set; }

    public string? A_no { get; set; }

    public string A_name { get; set; } = null!;

    public string? A_brief { get; set; }

    public string A_email { get; set; } = null!;

    public string A_password { get; set; } = null!;

    public string? A_line_id { get; set; }

    public bool A_forget_remind { get; set; }

    public bool A_is_admin {  get; set; }

    public string A_status { get; set; }

    public DateTime A_create { get; set; }

    public DateTime A_update { get; set; }

    public Account() 
    {
        A_status = "01";
        A_create = DateTime.Now;
        A_update = DateTime.Now;
    }
}
