using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataBase.Entities;

[Table("CodeTable")]
public partial class CodeTable
{
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid CT_id { get; set; }

    public string CT_type { get; set; } = null!;

    public string CT_desc { get; set; } = null!;

    public int CT_order { get; set; }

    public DateTime CT_create { get; set; }

    public DateTime CT_update { get; set; }
}
