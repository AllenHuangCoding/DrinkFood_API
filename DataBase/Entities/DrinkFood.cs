using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataBase.Entities;

[Table("DrinkFood")]
public partial class DrinkFood
{
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid DF_id { get; set; }

    public Guid DF_menu_id { get; set; }

    public string DF_name { get; set; } = null!;

    public Guid DF_type { get; set; }

    public int DF_price { get; set; }

    public string? DF_remark { get; set; }

    public string DF_status { get; set; }

    public DateTime DF_create { get; set; }

    public DateTime DF_update { get; set; }

    public DrinkFood() 
    { 
        DF_id = Guid.NewGuid();
        DF_status = "01";
        DF_create = DateTime.Now;
        DF_update = DateTime.Now;
    }
}
