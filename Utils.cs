using RetailLib.Enums;
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
        public static OperationType ToOperation(this (bool corr, bool ret, bool income) src) =>
            (OperationType)((src.corr ? 4 : 0) | (src.ret ? 2 : 0) | (src.income ? 1 : 0));
    }
}
