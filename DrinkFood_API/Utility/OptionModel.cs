using DataBase.Entities;

namespace DrinkFood_API.Utility
{
    public class OptionsModel
    {
        public Guid ID { get; set; }

        public string Value { get; set; }

        public OptionsModel(Brand Entity)
        {
            ID = Entity.B_id;
            Value = Entity.B_name;
        }

        public OptionsModel(Option Entity)
        {
            ID = Entity.O_id;
            Value = Entity.O_name;
        }
    }
}
