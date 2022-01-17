using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Camstar.Util
{
    public class DateUtil
    {
        public static char TimeSeparator = 'T';
        public static char PlusSign = '+';
        public static char MinusSign = '-';
        public static char GMTIndicator = 'Z';
        private const string ShortMonthPattern = "([1-9]|1[012])";
        private const string LongMonthPattern = "(0[1-9]|1[012])";
        private const string ShortDayPattern = "([1-9]|[12][0-9]|3[01])";
        private const string LongDayPattern = "([1-9]|[12][0-9]|3[01])";
        private const string ShortYearPattern = "([0-9][0-9])";
        private const string LongYearPattern = "((19|20)[0-9][0-9])";
        private const string ShortHourPattern = "([0-9]|1[012])";
        private const string LongHourPattern = "(0[1-9]|1[0-9]|2[0-4])";
        private const string ShortMinutePattern = "([0-9]|[12345][0-9])";
        private const string LongMinutePattern = "(0[0-9]|[12345][0-9])";
        private const string AMPMPattern = "(am|AM|pm|PM)";

        public static string GetCurrentUTCOffset() => DateTime.Now.ToString("%K");

        public static string StripTimeZoneOffset(string dateTimeWithOffset)
        {
            string[] strArray1 = dateTimeWithOffset != null ? dateTimeWithOffset.Split(DateUtil.TimeSeparator) : throw new ArgumentNullException(nameof(dateTimeWithOffset));
            string str1 = strArray1.Length == 2 ? strArray1[0].Trim() : throw new ArgumentException("Argument must contain 'T' in order to separate date and time.", nameof(dateTimeWithOffset));
            string str2 = strArray1[1].Trim();
            string str3 = str2;
            string str4 = string.Empty;
            if (str2.IndexOf(DateUtil.PlusSign) > 0)
            {
                string[] strArray2 = str2.Split(DateUtil.PlusSign);
                str3 = strArray2[0].Trim();
                str4 = DateUtil.PlusSign.ToString() + strArray2[1].Trim();
            }
            else if (str2.IndexOf(DateUtil.MinusSign) > 0)
            {
                string[] strArray2 = str2.Split(DateUtil.MinusSign);
                str3 = strArray2[0].Trim();
                str4 = DateUtil.MinusSign.ToString() + strArray2[1].Trim();
            }
            else if (str2.IndexOf(DateUtil.GMTIndicator) > 0)
                str3 = str2.Split(DateUtil.GMTIndicator)[0].Trim();
            return str1 + DateUtil.TimeSeparator.ToString() + str3;
        }

        public static DateTime ParseNoConvert(string dateTime)
        {
            dateTime = DateUtil.StripTimeZoneOffset(dateTime);
            return DateTime.Parse(dateTime);
        }

        public static bool IsDaylightSavingsActive()
        {
            var currentTimeZone = TimeZoneInfo.Local;
            return currentTimeZone.DaylightName != currentTimeZone.StandardName;
        }

        public static string DateFormat => Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern;

        public static string TimeFormat => Thread.CurrentThread.CurrentCulture.DateTimeFormat.LongTimePattern;

        public static string DateTimeFormat => string.Format("{0} {1}", DateUtil.DateFormat, DateUtil.TimeFormat);

        public static string DateTimeFormatPHP => string.Format("{0} {1}", DateUtil.DateFormat.ToLower().Replace("yyyy", "Y").Replace("yy", "y").Replace("d", "j").Replace("jj", "d").Replace("m", "n").Replace("nn", "m"), DateUtil.TimeFormat.ToLower().Replace("hh", "H").Replace("mm", "i").Replace("ss", "s").Replace("tt", "A").Replace("fff", "u"));

        public static string ApplyUserLocaleSettings(string message,string serverDateFormat,string serverTimeFormat)
        {
            string str = message;
            if (!string.IsNullOrEmpty(message) && !string.IsNullOrEmpty(serverDateFormat) && !string.IsNullOrEmpty(serverTimeFormat))
            {
                if (!string.Equals(serverDateFormat, DateUtil.DateFormat))
                {
                    string regExpr = new StringBuilder(serverDateFormat).Replace("MM", "(0[1-9]|1[012])").Replace("M", "([1-9]|1[012])").Replace("dd", "([1-9]|[12][0-9]|3[01])").Replace("d", "([1-9]|[12][0-9]|3[01])").Replace("yyyy", "((19|20)[0-9][0-9])").Replace("yy", "([0-9][0-9])").ToString();
                    message = DateUtil.FindAndReplace(message, regExpr, serverDateFormat, DateUtil.DateFormat);
                }
                if (!string.Equals(serverTimeFormat, DateUtil.TimeFormat))
                {
                    string regExpr = new StringBuilder(serverTimeFormat).Replace("hh", "(0[1-9]|1[0-9]|2[0-4])").Replace("h", "([0-9]|1[012])").Replace("mm", "(0[0-9]|[12345][0-9])").Replace("m", "([0-9]|[12345][0-9])").Replace("ss", "(0[0-9]|[12345][0-9])").Replace("s", "([0-9]|[12345][0-9])").Replace("tt", "(am|AM|pm|PM)").ToString();
                    message = DateUtil.FindAndReplace(message, regExpr, serverTimeFormat, DateUtil.TimeFormat);
                }
                str = message;
            }
            return str;
        }

        private static string FindAndReplace(
          string message,
          string regExpr,
          string inFormat,
          string outFormat)
        {
            string str1 = message;
            if (!string.IsNullOrEmpty(message))
            {
                MatchCollection source = new Regex(string.Format("\\b({0})\\b", regExpr)).Matches(message);
                if (source.Count > 0)
                {
                    foreach (string str2 in source.Cast<Match>().Select<Match, string>((Func<Match, string>)(dateTimeMatch => dateTimeMatch.Groups[0].Value)))
                    {
                        DateTime result;
                        if (DateTime.TryParseExact(str2, inFormat, (IFormatProvider)CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
                        {
                            string newValue = result.ToString(outFormat);
                            str1 = message.Replace(str2, newValue);
                        }
                    }
                }
            }
            return str1;
        }
    }
}