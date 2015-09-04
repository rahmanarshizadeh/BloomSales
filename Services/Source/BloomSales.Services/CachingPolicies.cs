using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace BloomSales.Services
{
    internal static class CachingPolicies
    {
        private static CacheItemPolicy oneMinute;
        private static CacheItemPolicy tenMinutes;
        private static CacheItemPolicy thirtyMinutes;
        private static CacheItemPolicy oneHour;
        private static CacheItemPolicy sixHours;
        private static CacheItemPolicy twelveHours;
        private static CacheItemPolicy oneDay;

        static CachingPolicies()
        {
            oneMinute = new CacheItemPolicy() { SlidingExpiration = new TimeSpan(0, 1, 0) };
            tenMinutes = new CacheItemPolicy() { SlidingExpiration = new TimeSpan(0, 10, 0) };
            thirtyMinutes = new CacheItemPolicy() { SlidingExpiration = new TimeSpan(0, 30, 0) };
            oneHour = new CacheItemPolicy() { SlidingExpiration = new TimeSpan(1, 0, 0) };
            sixHours = new CacheItemPolicy() { SlidingExpiration = new TimeSpan(6, 0, 0) };
            twelveHours = new CacheItemPolicy() { SlidingExpiration = new TimeSpan(12, 0, 0) };
            oneDay = new CacheItemPolicy() { SlidingExpiration = new TimeSpan(1, 0, 0, 0) };
        }

        public static CacheItemPolicy OneMinutePolicy
        {
            get { return oneMinute; }
        }

        public static CacheItemPolicy TenMinutesPolicy
        {
            get { return tenMinutes; }
        }

        public static CacheItemPolicy ThirtyMinutesPolicy
        {
            get { return thirtyMinutes; }
        }

        public static CacheItemPolicy OneHourPolicy
        {
            get { return oneHour; }
        }

        public static CacheItemPolicy SixHoursPolicy
        {
            get { return sixHours; }
        }

        public static CacheItemPolicy TwelveHoursPolicy
        {
            get { return twelveHours; }
        }

        public static CacheItemPolicy OneDayPolicy
        {
            get { return oneDay; }
        }
    }
}
