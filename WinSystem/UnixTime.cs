using System;
using System.Collections.Generic;
using System.Text;

namespace WinSystem
{
    /// <summary>
    /// 时间转换处理
    /// </summary>
    public class UnixTime
    {
        /// <summary>
        /// 将时间戳转换成DateTime
        /// </summary>
        /// <param name="timeStamp">时间戳</param>
        /// <returns>时间</returns>
        public static DateTime StampToDateTime(long timeStamp)
        {
            int count = timeStamp.ToString().Length;
            long timeStampl = timeStamp;
            switch (count)
            {
                case 13:
                    timeStampl *= 10000;
                    break;
                case 17:
                    break;
                case 18:
                    return DateTime.FromFileTimeUtc(timeStampl);
                default: return new DateTime();
            }
            //13或17，执行以下代码
            DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            TimeSpan toNow = new TimeSpan(timeStampl);
            return dateTimeStart.Add(toNow);
        }

        /// <summary>
        /// 将时间戳转换成DateTime
        /// </summary>
        /// <param name="timeStamp">时间戳</param>
        /// <returns>时间</returns>
        public static DateTime StampToDateTime(string timeStamp)
        {
            int count = timeStamp.Length;
            string timeStamps = timeStamp;
            switch (count)
            {
                case 13:
                    timeStamps += "0000";
                    break;
                case 17:
                    break;
                case 18:
                    return DateTime.FromFileTimeUtc(long.Parse(timeStamp));
                default: return new DateTime();
            }
            //13或17，执行以下代码
            try
            {
                DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                long lTime = long.Parse(timeStamps);
                TimeSpan toNow = new TimeSpan(lTime);
                return dateTimeStart.Add(toNow);
            }
            catch (Exception)
            {
                return new DateTime();
            }
            
        }
        /// <summary>
        /// DateTime时间格式转换为Unix时间戳格式（毫秒）
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>时间戳</returns>
        public static long DateTimeToStamp(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (long)(time - startTime).TotalMilliseconds;
        }
    }
}
