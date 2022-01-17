using System;
using System.Globalization;

namespace Camstar.XMLClient.API
{
    internal sealed class csiXMLDataFormat
    {
        private const string mkDateTimePattern = "yyyy-MM-dd'T'HH:mm:sszzz";
        private const string mkDatePattern = "yyyy-MM-dd";
        private const string mkTimePattern = "HH:mm:ss";
        private const string mkDecimalPattern = "0.0##############################";
        private const string mkFloatPattern = "#######0.0####################E0";

        private csiXMLDataFormat()
        {
        }

        internal static string GetUTCOffset() => DateTime.Now.ToString("zzz");

        internal static string locale2lexicalDateTime(string val) => DateTime.Parse(val).ToString("yyyy-MM-dd'T'HH:mm:sszzz");

        internal static string locale2lexicalDate(string val) => csiXMLDataFormat.locale2lexicalDateTime(val);

        internal static string locale2lexicalTime(string val) => csiXMLDataFormat.locale2lexicalDateTime(val);

        internal static string locale2lexicalDecimal(string val) => Decimal.Parse(val).ToString("0.0##############################");

        internal static string locale2lexicalFloat(string val) => double.Parse(val).ToString("#######0.0####################E0");

        internal static DateTime lexical2DateTime(string val) => DateTime.Parse(val);

        internal static string lexical2localeDateTime(string val)
        {
            DateTime dateTime = csiXMLDataFormat.lexical2DateTime(val);
            DateTimeFormatInfo dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat;
            return dateTime.ToString(dateTimeFormat.ShortDatePattern + " " + dateTimeFormat.LongTimePattern);
        }

        internal static string lexical2localeTime(string val) => csiXMLDataFormat.lexical2DateTime(val).ToString(CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern);

        internal static string lexical2localeDate(string val) => csiXMLDataFormat.lexical2DateTime(val).ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);

        internal static string lexical2localeDecimal(string val) => Decimal.Parse(val).ToString();

        internal static string lexical2localeFloat(string val) => double.Parse(val).ToString();
    }
}