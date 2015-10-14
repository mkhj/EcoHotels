using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcoHotels.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        /// <remarks>
        /// http://www.w3.org/TR/NOTE-datetime
        /// </remarks>
        public static string ToW3CTime(this DateTime date)
        {
            var utcOffset = TimeZone.CurrentTimeZone.GetUtcOffset(date);
            var w3CTime = date.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss");

            w3CTime += utcOffset == TimeSpan.Zero ? "Z" : String.Format("{0}{1:00}:{2:00}", (utcOffset > TimeSpan.Zero ? "+" : "-"), utcOffset.Hours, utcOffset.Minutes);

            return w3CTime;
        }

        public static int ToUnixTimestamp(this DateTime date)
        {
            //create Timespan by subtracting the value provided from the Unix Epoch
            var span = (date - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());

            //return the total seconds (which is a UNIX timestamp)
            return (int) span.TotalSeconds;            
        }
    }
}
