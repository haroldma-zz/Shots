using System;

namespace Shots.Core.Extensions
{
    public static class DateTimeHelper
    {
        public static DateTime EpochDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        ///     Returns a DateTime object from the unix timestamp.
        /// </summary>
        /// <param name="timestamp">The timestamp, in seconds.</param>
        /// <returns></returns>
        public static DateTime FromUnixTimestamp(this long timestamp)
        {
            return EpochDateTime.AddSeconds(timestamp).ToLocalTime();
        }

        /// <summary>
        ///     Return the a unix timestamp from a DateTime object.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        public static long ToUnixTimestamp(this DateTime dateTime)
        {
            if (dateTime.Kind != DateTimeKind.Utc)
                dateTime = dateTime.ToUniversalTime();
            return (long) (dateTime.Subtract(EpochDateTime)).TotalSeconds;
        }
    }
}