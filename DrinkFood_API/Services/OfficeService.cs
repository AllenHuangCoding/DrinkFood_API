using CodeShare.Libs.BaseProject;
using DrinkFood_API.Models;
using DrinkFood_API.Repository;

namespace DrinkFood_API.Services
{
    public class OfficeService : BaseService
    {
        [Inject] private readonly OfficeRepository _officeRepository;

        [Inject] private readonly OfficeMemberRepository _officeMemberRepository;

        public OfficeService(IServiceProvider provider) : base(provider)
        {
            provider.Inject(this);
        }

        public List<ResponseOfficeListModel> GetOfficeList(RequestOfficeListModel RequestData)
        {
            return _officeRepository.GetOfficeList();
        }

        public List<ResponseOfficeMemberListModel> GetOfficeMemberList(Guid OfficeID)
        {
            return _officeMemberRepository.GetOfficeMemberList(OfficeID);
        }

        public void PostOfficeMember(Guid OfficeID, RequestPostOfficeMemberModel RequestData)
        {
            _officeMemberRepository.PostOfficeMember(OfficeID, RequestData.AccountIDs);
        }

        public void DeleteOfficeMember(RequestDeleteOfficeMemberModel RequestData)
        {
            _officeMemberRepository.DeleteOfficeMember(RequestData.OfficeMemberIDs);
        }
    }
}
