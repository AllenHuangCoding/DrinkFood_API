using DataBase.Entities;
using DrinkFood_API.Models;

namespace DrinkFood_API.Utility
{
    public class OptionsModel
    {
        public Guid ID { get; set; }

        public string Text { get; set; }

        public OptionsModel(ResponseStoreListModel Entity)
        {
            ID = Entity.StoreID;
            Text = Entity.BrandStoreName;
        }

        public OptionsModel(CodeTable Entity)
        {
            ID = Entity.CT_id;
            Text = Entity.CT_desc;
        }

        public OptionsModel(Office Entity)
        {
            ID = Entity.O_id;
            Text = Entity.O_name;
        }

        public OptionsModel(Brand Entity)
        {
            ID = Entity.B_id;
            Text = Entity.B_name;
        }

        public OptionsModel(Option Entity)
        {
            ID = Entity.O_id;
            Text = Entity.O_name;
        }
    }
}
