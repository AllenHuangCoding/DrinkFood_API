using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataBase.View
{
    public class ViewStore
    {
        public Guid B_id { get; set; }

        public string B_name { get; set; } = null!;

        public string? B_line_id { get; set; }

        public string? B_logo { get; set; }

        public string? B_official_url { get; set; }

        public string B_type_desc { get; set; } = null!;

        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid S_id { get; set; }

        public Guid S_brand_id { get; set; }

        public Guid S_menu_area_id { get; set; }

        public string? S_line_id { get; set; }

        public string S_name { get; set; } = null!;

        public string S_address { get; set; } = null!;

        public string S_phone { get; set; } = null!;

        public string? S_remark { get; set; }

    }
}
