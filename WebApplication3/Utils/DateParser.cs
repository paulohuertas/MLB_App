using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace MLB_App.Utils
{
    public static class DateParser
    {
        public static string ParseDateToString(string date)
        {
            DateTime dateTime = DateTime.ParseExact(date, "yyyyMMdd", CultureInfo.InvariantCulture);
            string dt = dateTime.ToString("dd/MM/yyyy");
            return dt;
        }

        public static DateTime ConvertStringToDateTime(string date)
        {
            int year = int.Parse(date.Substring(0, 4));
            int month = int.Parse(date.Substring(4, 2));
            int day = int.Parse(date.Substring(6, 2));

            DateTime dt = new DateTime(year, month, day);

            return dt;
        }

        public static string ConvertTimeEpochToLocalTime(string epoch)
        {
            long longEpoch = long.Parse(epoch);
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(longEpoch);

            string completeDate = dateTimeOffset.LocalDateTime.ToString("hh:mm tt");
            return completeDate;
        }

        public static string ConvertTimeEpochToDate(string epoch)
        {
            long longEpoch = long.Parse(epoch);
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(longEpoch);

            string completeDate = dateTimeOffset.LocalDateTime.Date.ToString("dd/MM/yyyy");
            return completeDate;
        }
    }
}