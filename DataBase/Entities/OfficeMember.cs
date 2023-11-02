using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataBase.Entities;

[Table("OfficeMember")]
public partial class OfficeMember
{
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid OM_id { get; set; }

    public Guid OM_office_id { get; set; }

    public Guid OM_account_id { get; set; }

    public string OM_status { get; set; }

    public DateTime OM_create { get; set; }

    public DateTime OM_update { get; set; }

    public OfficeMember()
    {
        OM_id = Guid.NewGuid();
        OM_status = "01";
        OM_create = DateTime.Now;
        OM_update = DateTime.Now;
    }
}
