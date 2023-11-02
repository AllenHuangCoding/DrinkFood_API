using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataBase.Entities;

[Table("Office")]
public partial class Office
{
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid O_id { get; set; }

    public string O_name { get; set; } = null!;

    public string O_address { get; set; } = null!;

    public string O_status { get; set; } = null!;

    public Guid O_company_id { get; set; }

    public DateTime O_create { get; set; }

    public DateTime O_update { get; set; }
}
