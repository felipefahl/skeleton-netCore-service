using System;
using System.Collections.Generic;
using System.Linq;

namespace Skeleton.ServiceName.Utils.Helpers
{
    public static class DateTimeHelper
    {
        public static DateTime BrazilNow
        {
            get
            {
                return DateTime.Now.ToBrazilTime();
            }
        }

        public static DateTime ToBrazilTime(this DateTime date)
        {
            var brazilTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            return TimeZoneInfo.ConvertTime(date, TimeZoneInfo.Local, brazilTimeZoneInfo);
        }

        public static DateTime ToTimeZone(this DateTime date, string timeZoneId)
        {
            bool validTimeZone = TimeZoneInfo.GetSystemTimeZones().Any(x => x.Id == timeZoneId);
            if (validTimeZone)
            {
                var brazilTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                return TimeZoneInfo.ConvertTime(date, TimeZoneInfo.Local, brazilTimeZoneInfo);
            }
            else throw new InvalidTimeZoneException();
        }

        public static DateTime LastWorkDay(this DateTime date, List<DateTime> holidays)
        {
            var dt = date.Date;
            while (dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday || holidays.Any(y => y.Date == dt.Date))
            {
                dt = dt.AddDays(-1);
            }
            return dt;
        }
        public static int LastDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1).AddMonths(1).AddDays(-1).Day;
        }


        public static DateTime NextWorkDay(this DateTime date, List<DateTime> holidays)
        {
            var dt = date.Date;
            while (dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday || holidays.Any(y => y.Date == dt.Date))
            {
                dt = dt.AddDays(1);
            }
            return dt;
        }

        public static DateTime SafeAddMonths(this DateTime date, int months)
        {
            var dt = date;
            while (true)
            {
                try
                {
                    dt = dt.AddMonths(months);
                    break;
                }
                catch (Exception)
                {
                    dt = dt.AddDays(-1);
                }
            }
            return dt;
        }

        public static DateTime GetFromIsoString(string str)
        {
            return DateTime.Parse(str, null, System.Globalization.DateTimeStyles.RoundtripKind);
        }
    }
}
