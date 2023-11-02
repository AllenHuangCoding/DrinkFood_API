using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataBase.Entities;

[Table("AccountLogin")]
public partial class AccountLogin
{
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid AL_id { get; set; }

    public string AL_token { get; set; } = null!;

    public Guid AL_account_id { get; set; }

    public DateTime AL_create { get; set; }

    public DateTime AL_update { get; set; }
}
