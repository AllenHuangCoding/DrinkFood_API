using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CodeShare.Libs.GenericEntityFramework
{
    public class BaseView<TContext, TTable> : DQL<TContext, TTable> where TContext : ReadWriteEFContext, new() where TTable : class
    {
        /// <summary>
        /// 建構元
        /// </summary>
        /// <param name="provider"></param>
        public BaseView(IServiceProvider provider) : base(provider) { }

        #region 底層DQL查詢 (固定傳入AsNoTracking為true)

        /// <summary>
        /// 查詢全資料表 (AsNoTracking版)
        /// </summary>
        /// <returns></returns>
        public List<TTable> GetAll()
        {
            return GetAll(true);
        }

        /// <summary>
        /// 查詢全資料表並使用Where條件篩選 (AsNoTracking版)
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public List<TTable> FindAll(Expression<Func<TTable, bool>> match)
        {
            return FindAll(match, true);
        }

        /// <summary>
        /// 查詢全資料表並使用Where條件篩選 (AsNoTracking版)
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public TTable? FindOne(Expression<Func<TTable, bool>> match)
        {
            return FindOne(match, true);
        }

        /// <summary>
        /// 分頁查詢 (AsNoTracking版)
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="match"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public List<TTable> FindPage<TKey>(Expression<Func<TTable, bool>> match, Expression<Func<TTable, TKey>> orderBy, int pageSize, int pageIndex)
        {
            return FindPage(match, orderBy, pageSize, pageIndex, true);
        }

        #endregion
    }
}
