using DataBase.View;

namespace DrinkFood_API.Models
{
    public class RequestOfficeListModel
    {
    }

    public class ResponseOfficeListModel
    {
        public Guid OfficeID { get; set; }

        public string OfficeName { get; set; }

        public string OfficeAddress { get; set; }

        public Guid CompanyID { get; set; }

        public string CompanyName { get; set; }

        public ResponseOfficeListModel(ViewOffice Entity)
        {
            OfficeID = Entity.O_id;
            OfficeName = Entity.O_name;
            OfficeAddress = Entity.O_Address;
            CompanyID = Entity.O_company_id;
            CompanyName = Entity.C_name;
        }
    }

    public class RequestOfficeMemberListModel
    {
        public Guid OfficeID { get; set; }
    }

    public class ResponseOfficeMemberListModel
    {
        public Guid OfficeMemberID { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string? Brief {  get; set; }

        public ResponseOfficeMemberListModel(ViewOfficeMember Entity)
        {
            OfficeMemberID = Entity.OM_id;
            Name = Entity.A_name;
            Email = Entity.A_email;
            Brief = Entity.A_brief;
        }
    }

    public class RequestPostOfficeMemberModel
    {
        public List<Guid> AccountIDs { get; set; }
    }


    public class RequestDeleteOfficeMemberModel
    {
        public List<Guid> OfficeMemberIDs { get; set; }
    }

    public class ViewOfficeMember
    {
        public Guid OM_id { get; set; }

        public Guid OM_office_id { get; set; }

        public string O_name { get; set; } = null!;

        public string O_Address { get; set; } = null!;

        public Guid OM_account_id { get; set; }

        public string A_name { get; set; } = null!;

        public string? A_brief { get; set; }

        public string A_email { get; set; } = null!;
    }

    
}
