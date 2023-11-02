using Microsoft.EntityFrameworkCore;

namespace CodeShare.Libs.GenericEntityFramework
{
    public class BaseTable<TContext, TTable> : DML<TContext, TTable> where TContext : ReadWriteEFContext, new() where TTable : class
    {
        /// <summary>
        /// 建構元
        /// </summary>
        /// <param name="provider"></param>
        public BaseTable(IServiceProvider provider) : base(provider) { }
    }
}
