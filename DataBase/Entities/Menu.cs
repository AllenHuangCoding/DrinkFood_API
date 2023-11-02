using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataBase.Entities;

[Table("Menu")]
public partial class Menu
{
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid M_id { get; set; }

    public Guid M_brand_id { get; set; }

    public Guid M_area_id { get; set; }

    public string M_status { get; set; }

    public DateTime M_create { get; set; }

    public DateTime M_update { get; set; }

    public Menu()
    {
        M_id = Guid.NewGuid();
        M_status = "01";
        M_create = DateTime.Now;
        M_update = DateTime.Now;
    }
}
