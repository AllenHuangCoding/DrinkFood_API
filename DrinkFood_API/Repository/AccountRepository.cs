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

        public List<ViewAccount> GetAccountList() 
        {
            return GetViewAccount().ToList();
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

        public IQueryable<ViewAccount> GetViewAccount()
        {
            return from account in _readDBContext.Account
                   join drinkPayment in _readDBContext.CodeTable.Where(x => x.CT_type == "Payment") on account.A_default_drink_payment equals drinkPayment.CT_id into drinkPaymentGroup
                   from drinkPayment in drinkPaymentGroup.DefaultIfEmpty()
                   join lunchPayment in _readDBContext.CodeTable.Where(x => x.CT_type == "Payment") on account.A_default_lunch_payment equals lunchPayment.CT_id into lunchPaymentGroup
                   from lunchPayment in lunchPaymentGroup.DefaultIfEmpty()
                   select new ViewAccount
                   {
                       AccountID = account.A_id,
                       Name = account.A_name,
                       Brief = account.A_brief,
                       Email = account.A_email,
                       LineID = account.A_line_id,
                       IsAdmin = account.A_is_admin,
                       DefaultDrinkPayment = drinkPayment != null ? drinkPayment.CT_id : null,
                       DefaultDrinkPaymentDesc = drinkPayment != null ? drinkPayment.CT_desc : null,
                       DefaultLunchPayment = lunchPayment != null ? lunchPayment.CT_id : null,
                       DefaultLunchPaymentDesc = lunchPayment != null ? lunchPayment.CT_desc : null,
                   };
        }
    }
}
