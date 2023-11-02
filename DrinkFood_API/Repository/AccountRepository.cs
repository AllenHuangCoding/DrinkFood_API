using CodeShare.Libs.GenericEntityFramework;
using DataBase.Entities;
using DataBase;
using DrinkFood_API.Models;
using DrinkFood_API.Exceptions;

namespace DrinkFood_API.Repository
{
    public class AccountRepository : BaseTable<EFContext, Account>
    {
        /// <summary>
        /// 建構元
        /// </summary>
        public AccountRepository(IServiceProvider provider) : base(provider)
        {

        }

        public void UpdateProfile(UpdateProfileModel Data)
        {
            var account = GetById(Data.AccountID) ?? throw new ApiException("使用者ID不存在", 400);
            account.A_name = Data.AccountName;
            account.A_brief = Data.AccountBrief;
            account.A_update = DateTime.Now;
            Update(Data.AccountID, account);
        }

        public Account? Exist(string Number, string Password)
        {
            var account = FindOne(x => 
                x.A_no == Number && 
                x.A_password == Password && 
                x.A_status != "99"
            );

            if (account == null)
            {
                return null;
            }
            return account;
        }

        public Account? Exist(Guid AccountID)
        {
            var account = FindOne(x =>
                x.A_id == AccountID &&
                x.A_status != "99"
            );

            if (account == null)
            {
                return null;
            }
            return account;
        }
    }
}
