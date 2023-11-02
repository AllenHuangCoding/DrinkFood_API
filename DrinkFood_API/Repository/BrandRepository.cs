using CodeShare.Libs.GenericEntityFramework;
using DataBase.Entities;
using DataBase;
using DrinkFood_API.Models;
using DrinkFood_API.Utility;

namespace DrinkFood_API.Repository
{
    public class BrandRepository : BaseTable<EFContext, Brand>
    {
        /// <summary>
        /// 建構元
        /// </summary>
        public BrandRepository(IServiceProvider provider) : base(provider)
        {

        }

        public List<OptionsModel> GetBrandOption()
        {
            return FindAll(x => x.B_status != "99").Select(x => new OptionsModel(x)).ToList();
        }
    }
}
