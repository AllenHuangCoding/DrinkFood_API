using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataBase.Entities;

[Table("Brand")]
public partial class Brand
{
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid B_id { get; set; }

    public string B_name { get; set; } = null!;

    public string? B_line_id { get; set; }

    public string? B_logo { get; set; }

    public string? B_official_url { get; set; }

    public Guid B_type { get; set; }

    public double? B_tea_score { get; set; }

    public double? B_milktea_score { get; set; }

    public double? B_milkfoam_score { get; set; }

    public double? B_fruit_score { get; set; }

    public string B_status { get; set; }

    public DateTime B_create { get; set; }

    public DateTime B_update { get; set; }

    public Brand() 
    {
        B_status = "01";
        B_create = DateTime.Now;
        B_update = DateTime.Now;
    }
}
