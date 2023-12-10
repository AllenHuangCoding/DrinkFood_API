using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataBase.View
{
    public class ViewMenu
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid MenuID { get; set; }

        public Guid MenuAreaID { get; set; }

        public string MenuAreaDesc { get; set; } = null!;

        public Guid BrandID { get; set; }

        public string BrandName { get; set; } = null!;

        public DateTime MenuCreate { get; set; }
    }
}
