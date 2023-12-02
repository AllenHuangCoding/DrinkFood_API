using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataBase.Entities;

[Table("Company")]
public partial class Company
{
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid C_id { get; set; }

    public string C_name { get; set; } = null!;

    public string C_status { get; set; } = null!;

    public DateTime C_create { get; set; }

    public DateTime C_update { get; set; }
}
