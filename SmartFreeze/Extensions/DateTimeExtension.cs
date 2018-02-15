using System;

namespace SmartFreeze.Extensions
{
    public static class DateTimeExtension
    {
        public static bool IsSameDay(this DateTime current, DateTime date)
        {
            return current.Year == date.Year && current.Month == date.Month && current.Day == date.Day;
        }
    }
}
