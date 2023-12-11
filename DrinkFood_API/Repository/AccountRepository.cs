using CodeShare.Libs.GenericEntityFramework;
using DataBase.Entities;
using DataBase;
using DrinkFood_API.Models;
using CodeShare.Libs.BaseProject;
using DataBase.View;

namespace DrinkFood_API.Repository
{
    public class ViewAccountRepository : BaseView<EFContext, ViewAccount>
    {
        public ViewAccountRepository(IServiceProvider provider) : base(provider)
        {
            provider.Inject(this);
        }
    }

    public class AccountRepository : BaseTable<EFContext, Account>
    {
        /// <summary>
        /// 建構元
        /// </summary>
        public AccountRepository(IServiceProvider provider) : base(provider)
        {

        }

        #region 使用者查詢 (View)

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
