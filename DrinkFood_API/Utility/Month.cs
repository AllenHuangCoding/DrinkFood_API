using System.Globalization;

namespace DrinkFood_API.Utility
{
    public static class Month
    {
        public static List<string> GenerateDatesInMonth(int year, int month)
        {
            List<string> datesInMonth = new();

            DateTime startDate = new(year, month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);

            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                datesInMonth.Add(date.ToString("MM/dd"));
            }

            return datesInMonth;
        }

        public static List<string> GetDaysOfWeekInMonth(int year, int month)
        {
            List<string> daysOfWeek = new();

            DateTime startDate = new(year, month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);

            // 使用 CultureInfo 設定來獲得星期文字（例如：星期一、星期二）
            CultureInfo cultureInfo = CultureInfo.GetCultureInfo("zh-TW");

            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                string dayOfWeek = date.ToString("ddd", cultureInfo);
                daysOfWeek.Add(dayOfWeek);
            }

            return daysOfWeek;
        }
    }
}
