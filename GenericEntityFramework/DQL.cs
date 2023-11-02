using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CodeShare.Libs.GenericEntityFramework
{
    /// <summary>
    /// 資料查詢語言：DQL(Data Query Language)
    /// 負責進行資料查詢，不會對資料本身進行修改的語句。查詢/函數
    /// </summary>
    /// <typeparam name="TTable"></typeparam>
    public class DQL<TContext, TTable> where TContext : ReadWriteEFContext, new() where TTable : class
    {
        /// <summary>
        /// Read資料庫
        /// </summary>
        protected readonly TContext _readDBContext;

        /// <summary>
        /// 資料庫產生工廠
        /// </summary>

        [Inject] protected readonly DBContextFactory<TContext> _dbContextFactory;

        /// <summary>
        /// 建構元
        /// </summary>
        /// <param name="provider"></param>
        public DQL(IServiceProvider provider)
        {
            provider.Inject(this);

            if (_dbContextFactory == null)
            {
                throw new ArgumentException("DBContextFactory is not valid");
            }
            _readDBContext = _dbContextFactory.GetRandomReadDB();
        }

        #region 查詢

        /// <summary>
        /// 使用ID查詢特定資料 (Read DB)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual TTable? GetById<T>(T id)
        {
            return _readDBContext.Set<TTable>().Find(id);
        }

        /// <summary>
        /// 查詢全資料表
        /// </summary>
        /// <returns></returns>
        public List<TTable> GetAll(bool AsNoTracking = false)
        {
            if (AsNoTracking)
            {
                return _readDBContext.Set<TTable>().AsNoTracking().ToList();
            }
            return _readDBContext.Set<TTable>().ToList();
        }

        /// <summary>
        /// 查詢全資料表並使用Where條件篩選
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public List<TTable> FindAll(Expression<Func<TTable, bool>> match, bool AsNoTracking = false)
        {
            if (AsNoTracking)
            {
                return _readDBContext.Set<TTable>().AsNoTracking().Where(match).ToList();
            }
            return _readDBContext.Set<TTable>().Where(match).ToList();
        }

        /// <summary>
        /// 查詢全資料表並使用Where條件篩選
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public TTable? FindOne(Expression<Func<TTable, bool>> match, bool AsNoTracking = false)
        {
            if (AsNoTracking)
            {
                return _readDBContext.Set<TTable>().AsNoTracking().Where(match).FirstOrDefault();
            }
            return _readDBContext.Set<TTable>().Where(match).FirstOrDefault();
        }

        /// <summary>
        /// 分頁查詢
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="match"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public List<TTable> FindPage<TKey>(Expression<Func<TTable, bool>> match, Expression<Func<TTable, TKey>> orderBy, int pageSize, int pageIndex, bool AsNoTracking = false)
        {
            if (AsNoTracking)
            {
                return _readDBContext.Set<TTable>().AsNoTracking().Where(match).OrderByDescending(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            return _readDBContext.Set<TTable>().Where(match).OrderByDescending(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        #endregion

        #region 函數

        /// <summary>
        /// 資料筆數
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return _readDBContext.Set<TTable>().AsNoTracking().Count();
        }

        /// <summary>
        /// 資料筆數
        /// </summary>
        /// <returns></returns>
        public int Count(Expression<Func<TTable, bool>> match)
        {
            return _readDBContext.Set<TTable>().AsNoTracking().Where(match).Count();
        }

        // GroupBy

        // SelectMany

        #endregion
    }
}
