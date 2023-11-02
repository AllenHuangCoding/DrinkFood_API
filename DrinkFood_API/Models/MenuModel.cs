namespace DrinkFood_API.Models
{
    public class ViewMenu
    {
        public Guid MenuID { get; set; }

        public Guid MenuAreaID { get; set; }

        public string MenuAreaDesc { get; set; } = null!;

        public Guid BrandID { get; set; }

        public string BrandName { get; set; } = null!;

        public DateTime MenuCreate { get; set; }
    }
}
