using CodeShare.Libs.GenericEntityFramework;
using DataBase;
using DrinkFood_API.Models;
using DataBase.Entities;
using Microsoft.EntityFrameworkCore;

namespace DrinkFood_API.Repository
{
    public class OfficeMemberRepository : BaseTable<EFContext, OfficeMember>
    {
        /// <summary>
        /// 建構元
        /// </summary>
        public OfficeMemberRepository(IServiceProvider provider) : base(provider)
        {
            
        }

        public List<ResponseOfficeMemberListModel> GetOfficeMemberList(Guid OfficeID)
        {
            return GetViewOfficeMember().Where(x => 
                x.OM_office_id == OfficeID
            ).Select(x => new ResponseOfficeMemberListModel(x)).ToList();
        }

        public void PostOfficeMember(Guid OfficeID, List<Guid> AccountIDs)
        {
            foreach (Guid accountID in AccountIDs)
            {
                Create(new OfficeMember
                {
                    OM_office_id = OfficeID,
                    OM_account_id = accountID,
                });
            }
        }

        public void DeleteOfficeMember(List<Guid> OfficeMemberIDs)
        {
            // 套件改回傳IQueryAble後的寫法
            //FindAll(x => OfficeMemberIDs.Contains(x.OM_id));

            _writeDBContext.OfficeMember.Where(x => 
                OfficeMemberIDs.Contains(x.OM_id)
            ).ExecuteUpdate(x => 
                x.SetProperty(p => p.OM_status, "99").SetProperty(p => p.OM_update, DateTime.Now)
            );
        }

        public OfficeMember? Exist(Guid OfficeMemberID)
        {
            var officeMember = FindOne(x =>
                x.OM_id == OfficeMemberID &&
                x.OM_status != "99"
            );

            if (officeMember == null)
            {
                return null;
            }
            return officeMember;
        }

        private IQueryable<ViewOfficeMember> GetViewOfficeMember()
        {
            return from officeMember in _readDBContext.OfficeMember
                join office in _readDBContext.Office on officeMember.OM_office_id equals office.O_id
                join account in _readDBContext.Account on officeMember.OM_account_id equals account.A_id
                where officeMember.OM_status != "99" && office.O_status != "99" && account.A_status != "99"
                select new ViewOfficeMember
                {
                    OM_id = officeMember.OM_id,
                    OM_office_id = officeMember.OM_office_id,
                    O_name = office.O_name,
                    O_Address = office.O_address,
                    OM_account_id = officeMember.OM_account_id,
                    A_name = account.A_name,
                    A_brief = account.A_brief,
                    A_email = account.A_email,
                };
        }
    }
}
