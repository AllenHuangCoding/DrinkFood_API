using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataBase.Entities;

[Table("Option")]
public partial class Option
{
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid O_id { get; set; }

    public string O_status { get; set; } = null!;

    public string O_name { get; set; } = null!;

    public DateTime O_create { get; set; }

    public DateTime O_update { get; set; }
}
