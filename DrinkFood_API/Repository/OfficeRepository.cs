using CodeShare.Libs.GenericEntityFramework;
using DataBase;
using DrinkFood_API.Models;
using DataBase.Entities;

namespace DrinkFood_API.Repository
{
    public class OfficeRepository : BaseTable<EFContext, Office>
    {
        /// <summary>
        /// 建構元
        /// </summary>
        public OfficeRepository(IServiceProvider provider) : base(provider)
        {

        }

        public List<ResponseOfficeListModel> GetOfficeList()
        {
            return GetViewOffice().Select(x => new ResponseOfficeListModel(x)).ToList();
        }

        private IQueryable<ViewOffice> GetViewOffice()
        {
            return from office in _readDBContext.Office
                   join company in _readDBContext.Company on office.O_company_id equals company.C_id
                   where office.O_status != "99" && company.C_status != "99"
                   select new ViewOffice
                   {
                       O_id = office.O_id,
                       O_name = office.O_name,
                       O_Address = office.O_address,
                       O_company_id = office.O_company_id,
                       C_name = company.C_name,
                   };
        }
    }
}
