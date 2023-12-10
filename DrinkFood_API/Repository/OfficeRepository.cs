using CodeShare.Libs.GenericEntityFramework;
using DataBase;
using DrinkFood_API.Models;
using DataBase.Entities;
using DataBase.View;

namespace DrinkFood_API.Repository
{
    public class ViewOfficeRepository : BaseView<EFContext, ViewOffice>
    {
        public ViewOfficeRepository(IServiceProvider provider) : base(provider)
        {
            provider.Inject(this);
        }
    }

    public class OfficeRepository : BaseTable<EFContext, Office>
    {
        [Inject] private readonly ViewOfficeRepository _viewOfficeRepository;

        /// <summary>
        /// 建構元
        /// </summary>
        public OfficeRepository(IServiceProvider provider) : base(provider)
        {
            provider.Inject(this);
        }

        public List<ResponseOfficeListModel> GetOfficeList()
        {
            return _viewOfficeRepository.GetAll().Select(x => new ResponseOfficeListModel(x)).ToList();
        }
    }
}
