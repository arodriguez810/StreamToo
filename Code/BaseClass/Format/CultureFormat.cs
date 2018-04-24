using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Admin.BaseClass.Format
{
    public class CultureFormat
    {
        static NumberFormatInfo myNumberFormatInfo = new CultureInfo(CultureInfo.CurrentCulture.Name, false).NumberFormat;
        public static string CurrencyDecimalDigits(double money)
        {
            return Regex.Replace(money.ToString("C", CultureInfo.CurrentCulture), @"[^\d.]", "");
        }

        public static string getDateWithFormat(DateTime date)
        {
            string shortUsDateFormatString = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
            string shortUsTimeFormatString = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;
            return date.ToString(CultureInfo.CurrentCulture);
        }

        public static string getDateString(DateTime date)
        {
            return string.Format("{0}{1}{2}{3}{4}{5}", date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second,date.Millisecond);
        }


    }
}