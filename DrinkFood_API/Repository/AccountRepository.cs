using CodeShare.Libs.GenericEntityFramework;
using DataBase.Entities;
using DataBase;
using DrinkFood_API.Models;
using CodeShare.Libs.BaseProject;

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

        #region 使用者查詢 (View)

        public IQueryable<ViewAccount> GetViewAccount()
        {
            return from account in _readDBContext.Account
                   join drinkPayment in _readDBContext.CodeTable.Where(x => x.CT_type == "DrinkPayment") on account.A_default_drink_payment equals drinkPayment.CT_id into drinkPaymentGroup
                   from drinkPayment in drinkPaymentGroup.DefaultIfEmpty()
                   join lunchPayment in _readDBContext.CodeTable.Where(x => x.CT_type == "LunchPayment") on account.A_default_lunch_payment equals lunchPayment.CT_id into lunchPaymentGroup
                   from lunchPayment in lunchPaymentGroup.DefaultIfEmpty()
                   select new ViewAccount
                   {
                       AccountID = account.A_id,
                       Name = account.A_name,
                       Brief = account.A_brief,
                       Email = account.A_email,
                       LineID = account.A_line_id,
                       LunchNotify = account.A_lunch_notify,
                       DrinkNotify = account.A_drink_notify,
                       CloseNotify = account.A_close_notify,
                       DefaultDrinkPayment = drinkPayment != null ? drinkPayment.CT_id : null,
                       DefaultDrinkPaymentDesc = drinkPayment != null ? drinkPayment.CT_desc : null,
                       DefaultLunchPayment = lunchPayment != null ? lunchPayment.CT_id : null,
                       DefaultLunchPaymentDesc = lunchPayment != null ? lunchPayment.CT_desc : null,
                       IsAdmin = account.A_is_admin,
                   };
        }


        #endregion

        #region 修改個人資料

        public void UpdateProfile(UpdateProfileModel Data)
        {
            var account = GetById(Data.AccountID) ?? throw new ApiException("使用者ID不存在", 400);
            account.A_brief = Data.Brief;
            account.A_default_drink_payment = Data.DrinkDefaultPayment;
            account.A_default_lunch_payment = Data.LunchDefaultPayment;
            account.A_lunch_notify = Data.LunchNotify;
            account.A_drink_notify = Data.DrinkNotify;
            account.A_close_notify = Data.CloseNotify;
            account.A_update = DateTime.Now;
            Update(Data.AccountID, account);
        }

        #endregion

        #region Line相關功能


        #endregion

        #region 其他方法 (判斷使用者存在)

        public Account? Exist(string Email, string Password)
        {
            var account = FindOne(x =>
                x.A_email == Email &&
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

        #endregion
    }
}
