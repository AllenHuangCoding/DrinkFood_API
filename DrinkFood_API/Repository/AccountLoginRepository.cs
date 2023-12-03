using CodeShare.Libs.GenericEntityFramework;
using DataBase.Entities;
using DataBase;
using CodeShare.Libs.BaseProject;

namespace DrinkFood_API.Repository
{
    public class AccountLoginRepository : BaseTable<EFContext, AccountLogin>
    {
        /// <summary>
        /// 建構元
        /// </summary>
        public AccountLoginRepository(IServiceProvider provider) : base(provider)
        {

        }

        public void PatchDelete(Guid AccountID)
        {
            List<Guid> deleteIds = FindAll(x => x.AL_account_id == AccountID).Select(x => x.AL_id).ToList();
            foreach (Guid item in deleteIds)
            {
                Delete(item);
            }
        }

        public AccountLogin Exist(string token)
        {
            return FindOne(x => x.AL_token == token) ?? throw new ApiException("Token驗證失敗", 401);
        }
    }
}
