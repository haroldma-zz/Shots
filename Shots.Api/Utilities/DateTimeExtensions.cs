using System;

namespace Shots.Api.Utilities
{
    /// <summary>
    ///     Conatins extension method for working with DateTime objects
    /// </summary>
    public static class DateTimeExtensions
    {
        public static DateTime EpochDateTime = new DateTime(1970, 1, 1);

        /// <summary>
        /// Returns a DateTime object from the unix timestamp.
        /// </summary>
        /// <param name="timestamp">The timestamp.</param>
        /// <returns></returns>
        public static DateTime FromUnixTimestamp(this long timestamp)
        {
            return EpochDateTime.AddSeconds(timestamp);
        }

        /// <summary>
        /// Return the a unix timestamp from a DateTime object.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        public static long ToUnixTimestamp(this DateTime dateTime)
        {
            return (long) (dateTime.Subtract(EpochDateTime)).TotalSeconds;
        }
    }
}