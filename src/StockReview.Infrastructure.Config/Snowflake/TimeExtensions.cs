using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StockReview.Infrastructure.Config.Snowflake
{
    public static class TimeExtensions
    {
        public static Func<long> CurrentTimeFunc = InternalCurrentTimeMillis;

        public static long CurrentTimeMillis()
        {
            return CurrentTimeFunc();
        }

        /// <summary>
        ///     把Unix时间戳转化为时间
        /// </summary>
        /// <param name="unixTimeStamp"></param>
        /// <returns></returns>
        public static DateTime GetTimeByUnixTimestamp(this long unixTimeStamp, bool isAddEightHour = false)
        {
            var startTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc); // 当地时区
            var dt = startTime.AddSeconds(unixTimeStamp);
            if (isAddEightHour)
            {
                dt = dt.AddHours(8);
            }
            return dt;
        }

        public static IDisposable StubCurrentTime(Func<long> func)
        {
            CurrentTimeFunc = func;
            return new DisposableAction(() =>
            {
                CurrentTimeFunc = InternalCurrentTimeMillis;
            });
        }

        public static IDisposable StubCurrentTime(long millis)
        {
            CurrentTimeFunc = () => millis;
            return new DisposableAction(() =>
            {
                CurrentTimeFunc = InternalCurrentTimeMillis;
            });
        }

        private static readonly DateTime Jan1st1970 = new DateTime
           (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private static long InternalCurrentTimeMillis()
        {
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }
    }
}