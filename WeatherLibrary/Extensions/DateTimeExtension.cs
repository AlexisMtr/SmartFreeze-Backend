using System;

namespace WeatherLibrary.Extensions
{
    public static class DateTimeExtension
    {
        private static DateTime januaryFirst = new DateTime(1970, 1, 1, 0, 0, 0);

        public static DateTime ToDateTime(this long timestamp)
        {
            return januaryFirst.AddSeconds(timestamp);
        }

        public static long ToTimestamp(this DateTime datetime)
        {
            return Convert.ToInt64((datetime - januaryFirst).TotalSeconds);
        }
    }
}
