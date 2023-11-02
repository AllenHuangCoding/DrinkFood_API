using Microsoft.Extensions.DependencyInjection;

namespace CodeShare.Libs.GenericEntityFramework
{
    /// <summary>
    /// 寫入Log的介面定義
    /// </summary>
    public interface IDmlLogService
    {
        /// <summary>
        /// 操作資料成功的Log紀錄
        /// </summary>
        Action SuccessLogCallback();

        /// <summary>
        /// 操作資料失敗的Log紀錄
        /// </summary>
        Action<Exception?> FailLogCallback();
    }

    /// <summary>
    /// 資料操作語言：DML(Data Manipulation Language)
    /// 用來處理資料表裡的資料。新增/修改/刪除
    /// </summary>
    /// <typeparam name="TTable"></typeparam>
    public class DML<TContext, TTable> : DQL<TContext, TTable> where TContext : ReadWriteEFContext, new() where TTable : class
    {
        [Inject] protected readonly IDmlLogService _dmlLogService;

        /// <summary>
        /// Write資料庫
        /// </summary>
        protected readonly TContext _writeDBContext;

        /// <summary>
        /// 建構元
        /// </summary>
        /// <param name="provider"></param>
        public DML(IServiceProvider provider) : base(provider)
        {
            provider.Inject(this);
            _writeDBContext = _dbContextFactory.GetWriteDB();

            if (_dmlLogService == null)
            {
                throw new ArgumentException("IDmlLogService is not valid");
            }
        }


        /// <summary>
        /// 使用ID查詢特定資料 (Write DB)
        /// 在Update、Delete方法中，必須從Write DB中用ID查出資料
        /// 如果從Read DB中查資料會無法做後續的資料操作
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override TTable? GetById<T>(T id)
        {
            return _writeDBContext.Set<TTable>().Find(id);
        }

        #region 新增

        /// <summary>
        /// 單筆新增
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public TTable Create(TTable instance)
        {
            try
            {
                if (instance == null)
                {
                    _dmlLogService.FailLogCallback()?.Invoke(null);
                    throw new ArgumentNullException(nameof(instance));
                }
                else
                {
                    _writeDBContext.Set<TTable>().Add(instance);
                    _writeDBContext.SaveChanges();
                    _dmlLogService.SuccessLogCallback()?.Invoke();
                    return instance;
                }
            }
            catch (Exception ex)
            {
                _dmlLogService.FailLogCallback()?.Invoke(ex);
                throw new ArgumentNullException(ex.Message);
            }
        }

        /// <summary>
        /// 多筆新增
        /// </summary>
        /// <param name="instances"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public List<TTable> CreateRange(List<TTable> instances)
        {
            try
            {
                if (instances == null)
                {
                    _dmlLogService.FailLogCallback()?.Invoke(null);
                    throw new ArgumentNullException(nameof(instances));
                }
                else
                {
                    foreach (var item in instances)
                    {
                        _writeDBContext.Set<TTable>().Add(item);
                    }
                    _writeDBContext.SaveChanges();
                    _dmlLogService.SuccessLogCallback()?.Invoke();
                    return instances;
                }
            }
            catch (Exception ex)
            {
                _dmlLogService.FailLogCallback()?.Invoke(ex);
                throw new ArgumentNullException(ex.Message);
            }
        }

        #endregion

        #region 修改

        /// <summary>
        /// 修改資料
        /// </summary>
        /// <param name="id"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public TTable Update<T>(T id, TTable instance)
        {
            try
            {
                if (instance == null)
                {
                    _dmlLogService.FailLogCallback()?.Invoke(null);
                    throw new ArgumentNullException(nameof(instance));
                }
                else
                {
                    TTable? existing = GetById(id);
                    if (existing == null)
                    {
                        _dmlLogService.FailLogCallback()?.Invoke(null);
                        throw new ArgumentNullException(nameof(id));
                    }

                    _writeDBContext.Set<TTable>().Update(instance);
                    _writeDBContext.SaveChanges();
                    _dmlLogService.SuccessLogCallback()?.Invoke();
                    return existing;
                }
            }
            catch (Exception ex)
            {
                _dmlLogService.FailLogCallback()?.Invoke(ex);
                throw new ArgumentNullException(ex.Message);
            }
        }

        #endregion

        #region 刪除

        /// <summary>
        /// 刪除特定單筆資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public bool Delete<T>(T id)
        {
            try
            {
                if (id == null)
                {
                    _dmlLogService.FailLogCallback()?.Invoke(null);
                    throw new ArgumentNullException(nameof(id));
                }
                else
                {
                    TTable? instance = GetById(id);
                    if (instance == null)
                    {
                        _dmlLogService.FailLogCallback()?.Invoke(null);
                        throw new ArgumentNullException(nameof(id));
                    }
                    _writeDBContext.Set<TTable>().Remove(instance);
                    _writeDBContext.SaveChanges();
                    _dmlLogService.SuccessLogCallback()?.Invoke();
                    return true;
                }
            }
            catch (Exception ex)
            {
                _dmlLogService.FailLogCallback()?.Invoke(ex);
                throw new ArgumentNullException(ex.Message);
            }
        }

        #endregion

    }
}
