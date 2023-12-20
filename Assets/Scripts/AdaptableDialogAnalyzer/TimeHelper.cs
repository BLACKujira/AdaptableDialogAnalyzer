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

        /// <summary>
        /// 将Datetime转换为Unix时间戳
        /// </summary>
        public static long DateTimeToUnixTimeMS(DateTime dateTime)
        {
            return new DateTimeOffset(dateTime).ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// 将Datetime转换为Unix时间戳（使用东京标准时间）
        /// </summary>
        public static long DateTimeToUnixTimeMSTST(DateTime dateTime)
        {
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
            DateTimeOffset dateTimeOffset = TimeZoneInfo.ConvertTime(dateTime, timeZoneInfo);
            return dateTimeOffset.ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// 将Datetime转换为Unix时间戳（使用东京标准时间）
        /// </summary>
        public static long DateTimeToUnixTimeTST(DateTime dateTime)
        {
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
            DateTimeOffset dateTimeOffset = TimeZoneInfo.ConvertTime(dateTime, timeZoneInfo);
            return dateTimeOffset.ToUnixTimeSeconds();
        }
    }
}