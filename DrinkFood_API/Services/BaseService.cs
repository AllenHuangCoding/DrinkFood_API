using DataBase;
using DrinkFood_API.Model;

namespace DrinkFood_API.Service
{
    public class BaseService
    {
        public SimpleResult SimpleResult { get; set; } = new SimpleResult();

        /// <summary>
        /// 封包資訊
        /// </summary>
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public BaseService()
        {

        }

        public BaseService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
