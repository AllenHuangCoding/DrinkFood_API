using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataBase.Entities;

[Table("OptionDetail")]
public partial class OptionDetail
{
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid OD_id { get; set; }

    public Guid OD_drink_food_id { get; set; }

    public Guid OD_brand_id { get; set; }

    public Guid OD_option_id { get; set; }

    public DateTime OD_create { get; set; }

    public DateTime OD_update { get; set; }
}
