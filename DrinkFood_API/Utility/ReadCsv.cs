using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using System.Text;

namespace DrinkFood_API.Utility
{
    public static class ReadCsv
    {
        public static void Convert<T>(IFormFile File, Action<List<T>> Callback) where T : class
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                MissingFieldFound = null
            };

            using (var ms = new MemoryStream())
            {
                File.CopyToAsync(ms);
                ms.Seek(0, SeekOrigin.Begin);

                using (var reader = new StreamReader(ms, Encoding.GetEncoding("big5")))
                {
                    using (var csv = new CsvReader(reader, config))
                    {
                        var records = csv.GetRecords<T>().ToList();
                        Callback.Invoke(records);
                    }
                }
            }
        }
    }
}
