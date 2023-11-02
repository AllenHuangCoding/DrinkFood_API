using CodeShare.Libs.GenericEntityFramework;
using DataBase.Entities;
using DataBase;

namespace DrinkFood_API.Repository
{
    public class CodeTableRepository : BaseView<EFContext, CodeTable>
    {
        /// <summary>
        /// 建構元
        /// </summary>
        public CodeTableRepository(IServiceProvider provider) : base(provider)
        {

        }

        public List<CodeTable> GetListByType(string Type)
        {
            return FindAll(x => x.CT_type == Type).OrderBy(x => x.CT_order).ToList();
        }
    }
}
