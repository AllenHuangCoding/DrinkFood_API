using Microsoft.EntityFrameworkCore;

namespace CodeShare.Libs.GenericEntityFramework
{
    /// <summary>
    /// 擴充資料庫設定連線方法
    /// </summary>
    public class ReadWriteEFContext : DbContext
    {
        /// <summary>
        /// DbContext預設方法複寫
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer();
        }
    }
}
