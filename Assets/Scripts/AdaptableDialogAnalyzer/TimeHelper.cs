using System;

namespace AdaptableDialogAnalyzer
{
    public static class TimeHelper
    {
        /// <summary>
        /// 注意时区问题
        /// </summary>
        public static DateTime UnixTimeMSToDateTime(long unixTime)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(unixTime).DateTime;
        }

        /// <summary>
        /// 以东京时间计算
        /// </summary>
        public static DateTime UnixTimeMSToDateTimeTST(long unixTime)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(unixTime);
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
            return TimeZoneInfo.ConvertTime(dateTimeOffset, timeZoneInfo).DateTime;
        }
    }
}