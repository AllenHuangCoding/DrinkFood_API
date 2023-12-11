using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataBase.View
{
    public class ViewOffice
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid O_id { get; set; }

        public string O_name { get; set; } = null!;

        public string O_Address { get; set; } = null!;

        public Guid O_company_id { get; set; }

        public string C_name { get; set; } = null!;
    }
}
