using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataBase.Entities;

[Table("Store")]
public partial class Store
{
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid S_id { get; set; }

    public Guid S_brand_id { get; set; }

    public string? S_line_id { get; set; }

    public string S_name { get; set; } = null!;

    public string S_address { get; set; } = null!;

    public string S_phone { get; set; } = null!;

    public Guid S_menu_area_id { get; set; }

    public string S_status { get; set; }

    public DateTime S_create { get; set; }

    public DateTime S_update { get; set; }

    public Store()
    {
        S_status = "01";
        S_create = DateTime.Now;
        S_update = DateTime.Now;
    }

}
