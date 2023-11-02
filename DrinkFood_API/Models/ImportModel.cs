namespace DrinkFood_API.Models
{
    public class BrandStoreCsvColumn
    {
        public string 品牌名字 { get; set; }
        public string 品牌Line { get; set; }
        public string 品牌Logo網址 { get; set; }
        public string 品牌官網網址 { get; set; }
        public string 品牌類型 { get; set; }
        public string 店家名 { get; set; }
        public string 店家地址 { get; set; }
        public string 店家電話 { get; set; }
        public string 菜單區域 { get; set; }
    }

    public class OrderDetailCsvColumn
    {
        public string Name { get; set; }

        public string DrinkFoodName { get; set; }

        public string Size { get; set; }

        public string Sugar { get; set; }

        public string Ice { get; set; }

        public string Payment { get; set; }

        public string AddItem { get; set; }

        public int Price { get; set; }
    }
}
