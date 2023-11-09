﻿namespace DrinkFood_API.Models
{
    public class RequestStoreListModel
    {
        public string? SearchKey { get; set; }
    }

    public class ViewStore
    {
        public Guid B_id { get; set; }

        public string B_name { get; set; } = null!;

        public string? B_line_id { get; set; }

        public string? B_logo { get; set; }

        public string? B_official_url { get; set; }

        public string B_type_desc { get; set; } = null!;

        public Guid S_id { get; set; }

        public Guid S_brand_id { get; set; }

        public Guid S_menu_area_id { get; set; }

        public string? S_line_id { get; set; }

        public string S_name { get; set; } = null!;

        public string S_address { get; set; } = null!;

        public string S_phone { get; set; } = null!;

    }

    /// <summary>
    /// 店家清單
    /// </summary>
    public class ResponseStoreListModel
    {
        public Guid BrandID { get; set; }

        public string BrandName { get; set; }

        public string? BrandLogoUrl { get; set; }

        public string? BrandOfficialUrl { get; set; }

        public string BrandTypeDesc { get; set; }

        public Guid StoreID { get; set; }

        public Guid StoreMenuAreaID { get; set; }

        public string StoreName { get; set; }

        public string StoreAddress { get; set; }

        public string StorePhone { get; set; }

        public string PreviousOrderDate { get; set; }

        /// <summary>
        /// 若品牌有統一Line帳號則優先呈現，之後才是呈現店家Line帳號 (B_line_id > S_line_id)
        /// </summary>
        public string? LineID { get; set; }

        public ResponseStoreListModel(ViewStore Entity)
        {
            BrandID = Entity.B_id;
            BrandName = Entity.B_name;
            BrandLogoUrl = Entity.B_logo;
            BrandOfficialUrl = Entity.B_official_url;
            BrandTypeDesc = Entity.B_type_desc;
            StoreID = Entity.S_id;
            StoreMenuAreaID = Entity.S_menu_area_id;
            StoreName = Entity.S_name;
            StoreAddress = Entity.S_address;
            StorePhone = Entity.S_phone;
            PreviousOrderDate = "-";
            LineID = Entity.B_line_id ?? Entity.S_line_id ?? null;
        }
    }
}
