namespace CodeShare.Libs.BaseProject.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime DayBegin(this DateTime param)
        {
            return Convert.ToDateTime(param.ToString("yyyy-MM-dd 00:00:00"));
        }

        public static DateTime DayEnd(this DateTime param)
        {
            return Convert.ToDateTime(param.ToString("yyyy-MM-dd 23:59:59"));
        }

        public static string ToDateHourMinute(this DateTime param)
        {
            return param.ToString("yyyy-MM-dd HH:mm");
        }
    }
}
