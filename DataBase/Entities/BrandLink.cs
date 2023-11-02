using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataBase.Entities;

[Table("BrandLink")]
public partial class BrandLink
{
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid BL_id { get; set; }

    public string BL_link_url { get; set; } = null!;

    public string? BL_message { get; set; }

    public string BL_status { get; set; } = null!;

    public Guid BL_create_account_id { get; set; }

    public DateTime BL_create { get; set; }

    public DateTime BL_update { get; set; }
}
