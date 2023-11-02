using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CodeShare.Libs.GenericEntityFramework
{
    /// <summary>
    /// 產生 Read/Write 資料庫工廠
    /// </summary>
    public class DBContextFactory<TContext> where TContext : ReadWriteEFContext, new()
    {
        /// <summary>
        /// Write資料庫 (單個)
        /// </summary>
        private readonly TContext _writeDB;

        /// <summary>
        /// Read資料庫 (多個)
        /// </summary>
        private readonly List<TContext> _readDB;

        /// <summary>
        /// 建構元
        /// </summary>
        /// <param name="configuration"></param>
        /// <exception cref="ArgumentException"></exception>
        public DBContextFactory(IConfiguration configuration)
        {
            // 判斷Read資料庫連線字串
            var readDBConfigChildren = configuration.GetSection("ConnectionStrings:ReadDB").GetChildren().ToList();
            if (readDBConfigChildren.Count == 0)
            {
                throw new ArgumentException("appsettings.json -> Key ConnectionStrings -> Array<string> ReadDB is not valid");
            }

            // 過濾Read連線空字串
            List<string?> readDBList = readDBConfigChildren.Select(x => x.Value).ToList();
            if (readDBList.Where(x => !string.IsNullOrWhiteSpace(x)).ToList().Count == 0)
            {
                throw new ArgumentException("Array<string> ReadDB content is not valid");
            }

            // 判斷Write資料庫連線字串
            string? writeDBConfig = configuration.GetConnectionString("WriteDB");
            if (string.IsNullOrWhiteSpace(writeDBConfig))
            {
                throw new ArgumentException("appsettings.json -> Key ConnectionStrings -> String WriteDB");
            }

            // 產生全部的Read / Write資料庫
            _readDB = readDBList.Select(x => CreateContext(x!.Trim())).ToList();
            _writeDB = CreateContext(writeDBConfig);
        }

        /// <summary>
        /// 回傳Write資料庫物件
        /// </summary>
        /// <returns></returns>
        public TContext GetWriteDB() 
        {
            return _writeDB;
        }

        /// <summary>
        /// 回傳隨機取樣的Read資料庫物件 (隨機策略、權重策略、輪詢策略)
        /// </summary>
        /// <returns></returns>
        public TContext GetRandomReadDB()
        {
            return _readDB[new Random().Next(0, _readDB.Count)];
        }

        /// <summary>
        /// 產生資料庫實體
        /// </summary>
        /// <param name="connectionString">連線字串</param>
        /// <returns></returns>
        private TContext CreateContext(string connectionString)
        {
            TContext context = new();
            context.Database.SetConnectionString(connectionString);
            return context;
        }
    }
}
