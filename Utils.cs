using System;
using System.Globalization;

namespace RetailLib
{
    public static class Utils
    {
        public static string ToISO(this DateTime dt) =>
            dt.ToString("yyyy-MM-dd");
        public static string ToFullISO(this DateTime dt) =>
            dt.ToString("yyyy-MM-ddTHH:mm:ss");
        public static DateTime ParseJSONDateTime(this string text) =>
            DateTime.ParseExact(text, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
    }
}
