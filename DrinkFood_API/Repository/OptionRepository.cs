using CodeShare.Libs.GenericEntityFramework;
using DataBase.Entities;
using DataBase;

namespace DrinkFood_API.Repository
{
    public class OptionRepository : BaseView<EFContext, Option>
    {
        /// <summary>
        /// 建構元
        /// </summary>
        public OptionRepository(IServiceProvider provider) : base(provider)
        {

        }

    }
}
