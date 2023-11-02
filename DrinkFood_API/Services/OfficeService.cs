using DrinkFood_API.Models;
using DrinkFood_API.Repository;
using DrinkFood_API.Service;

namespace DrinkFood_API.Services
{
    public class OfficeService : BaseService
    {
        [Inject] private readonly OfficeRepository _officeRepository;

        [Inject] private readonly OfficeMemberRepository _officeMemberRepository;

        public OfficeService(IServiceProvider provider) : base()
        {
            provider.Inject(this);
        }

        public List<ResponseOfficeListModel> GetOfficeList(RequestOfficeListModel Request)
        {
            return _officeRepository.GetOfficeList();
        }

        public List<ResponseOfficeMemberListModel> GetOfficeMemberList(Guid OfficeID)
        {
            return _officeMemberRepository.GetOfficeMemberList(OfficeID);
        }

        public void PostOfficeMember(Guid OfficeID, RequestPostOfficeMemberModel Request)
        {
            _officeMemberRepository.PostOfficeMember(OfficeID, Request.AccountIDs);
        }

        public void DeleteOfficeMember(RequestDeleteOfficeMemberModel Request)
        {
            _officeMemberRepository.DeleteOfficeMember(Request.OfficeMemberIDs);
        }
    }
}
