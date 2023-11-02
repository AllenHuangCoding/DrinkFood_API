using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CodeShare.Libs.GenericEntityFramework
{
    public static class ProgramExtension
    {
        public static void AddScopedByClassName(this IServiceCollection serviceCollection, string className)
        {
            /*
            GetExecutingAssembly: 取得組件，其中含有目前正在執行的程式碼。
            GetCallingAssembly: 傳回方法的 Assembly，其叫用目前執行的方法。
            GetEntryAssembly: 取得預設應用程式定義域中的處理序可執行檔。(從 Unmanaged 程式碼呼叫時，可能會傳回 null。)
            Type.GetType: 取得具有指定名稱的 Type

            |   File  |                 Method                 | Result |
            |-----------------------------------------------------------|
            | Program | GetExecutingAssembly -> Type.GetType() |    O   |
            | Static  | GetExecutingAssembly -> Type.GetType() |    X   |
            | Program | GetCallingAssembly   -> Type.GetType() |    X   |
            | Static  | GetCallingAssembly   -> Type.GetType() |    X   |
            | Program | GetEntryAssembly     -> Type.GetType() |    O   |
            | Static  | GetEntryAssembly     -> Type.GetType() |    O   |
            */

            // 判斷傳入搜尋的Class名稱
            if (string.IsNullOrWhiteSpace(className))
            {
                throw new ArgumentNullException(nameof(className));
            }

            // 取得處理序可執行檔
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly != null)
            {
                // 包含這個組件的完整剖析顯示名稱
                var assemblyName = entryAssembly.GetName().Name;
                if (assemblyName != null)
                {
                    // 取得這個元件中定義的所有型別。
                    var entryGetTypes = entryAssembly.GetTypes();

                    // 篩選出符合搜尋條件的資料
                    var entryBaseTable = entryGetTypes.Where(x => x.BaseType != null && x.BaseType.ToString().Contains(className));

                    // 將組件的名稱轉換為 Type
                    var entryGetType = entryBaseTable.Select(x => Type.GetType(string.Format("{0}, {1}", x.FullName, assemblyName))).ToList();

                    // 若 Type 轉換成功才加入ServiceCollection
                    foreach (var item in entryGetType.Where(x => x != null).Select(x => x!).ToList())
                    {
                        serviceCollection.AddScoped(item);
                    }
                }
            }
        }
    }
}
