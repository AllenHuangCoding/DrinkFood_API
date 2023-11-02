using CodeShare.Libs.GenericEntityFramework;
using DataBase;

namespace DrinkFood_API.Service
{
    public class LogService : IDmlLogService
    {
        public LogService()
        {

        }

        public Action SuccessLogCallback()
        {
            return () => {
                Console.WriteLine("寫入資料表成功");
            };
        }

        public Action<Exception?> FailLogCallback()
        {
            return (exception) => {
                Console.WriteLine("失敗資訊：" + exception?.Message + exception?.StackTrace);
            };
        }
    }
}
