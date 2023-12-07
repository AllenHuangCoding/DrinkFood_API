namespace CodeShare.Libs.BaseProject.Extensions
{
    public class ResponsePaginationModel<T>
    {
        public int TotalCount { get; set; }

        public int Count { get; set; }

        public List<T> Data { get; set; }
    }

    /// <summary>
    /// List擴充方法
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// List<T>泛型的記憶體處理分頁
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="allData"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static ResponsePaginationModel<T> MemoryPagination<T>(this List<T> allData, int offset = 0, int limit = 0)
        {
            if (offset == 0 && limit == 0)
            {
                // 皆為預設值，不做任何過濾
                ResponsePaginationModel<T> response = new()
                {
                    TotalCount = allData.Count,
                    Data = allData
                };
                response.Count = response.Data.Count;
                return response;
            }
            else
            {
                // 有設定Offset & Limit才會進行過濾
                ResponsePaginationModel<T> response = new()
                {
                    TotalCount = allData.Count,
                    Data = allData.Skip(offset).Take(limit).ToList()
                };
                response.Count = response.Data.Count;
                return response;
            }
        }

        public static List<TResult> SelectProperty<TSource, TResult>(
            this List<TSource> source,
            Func<TSource, TResult> selector
        )
        {
            return source.Select(selector).OrderBy(x => x).ToList();
        }

        
        public static List<TSource> ContainsProperty<TSource, TProperty>(
            this IEnumerable<TSource> source,
            IEnumerable<TProperty> values,
            Func<TSource, TProperty> propertySelector
        )
        {
            var valuesSet = new HashSet<TProperty>(values);
            return source.Where(item => valuesSet.Contains(propertySelector(item))).ToList();
        }
    }
}
