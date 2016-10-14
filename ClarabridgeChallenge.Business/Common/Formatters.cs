using System;
using System.Text.RegularExpressions;
using System.Threading;

namespace ClarabridgeChallenge.Business.Common
{
    public static class Formatters
    {
        #region Text Trimming
        public static string TrimText(string textToTrim)
        {
            //First ensure the textToTrim is trimmed
            textToTrim = textToTrim.Trim();
            string sAppendedText = " ...";
            int maxLettersToShow = Convert.ToInt32(CommonFunctions.GetApplicationSetting("TrimmedTextLength", "300")) - sAppendedText.Length;
            //If the text is less than the limit we don't need to trim the text
            if (textToTrim.Length <= maxLettersToShow)
            {
                return textToTrim;
            }

            //As we are not forcing the exact length
            //we first find the index of the last '.', if its within the last 20 characters then we return the text upto and including this '.' this makes it cleaner
            string trimmedText = textToTrim.Substring(0, maxLettersToShow);
            int lastIndexOfPeriod = trimmedText.LastIndexOf(".");
            if (lastIndexOfPeriod > maxLettersToShow - 50)
            {
                return trimmedText.Substring(0, lastIndexOfPeriod) + ".";
            }
            else
            {
                trimmedText = textToTrim.Substring(0, maxLettersToShow - 3).Trim();
                int lastIndexOfWhiteSpace = trimmedText.LastIndexOf(" ");
                trimmedText = textToTrim.Substring(0, lastIndexOfWhiteSpace);
                return trimmedText + sAppendedText;
            }
        }
        #endregion

        #region Datetime

        public struct DateTimeFormats
        {
            #region Format All_Bold
            public static string FormatFullDate_WithTime_AllBold(DateTime dtToFormat)
            {
                return FormatFullDate_WithTime("<b>{0} {1}</b>", dtToFormat);
            }

            public static string FormatFullDate_NoTime_AllBold(DateTime dtToFormat)
            {
                return FormatFullDate_NoTime("<b>{0}</b>", dtToFormat);
            }

            public static string FormatShortDate_WithTime_AllBold(DateTime dtToFormat)
            {
                return FormatShortDate_WithTime("<b>{0} {1}</b>", dtToFormat);
            }

            public static string FormatShortDate_NoTime_AllBold(DateTime dtToFormat)
            {
                return FormatShortDate_NoTime("<b>{0}</b>", dtToFormat);
            }
            #endregion

            #region Format Plain
            public static string FormatFullDate_WithTime(DateTime dtToFormat)
            {
                return FormatFullDate_WithTime("{0} {1}", dtToFormat);
            }

            public static string FormatFullDate_NoTime(DateTime dtToFormat)
            {
                return FormatFullDate_NoTime("{0}", dtToFormat);
            }

            public static string FormatShortDate_WithTime(DateTime dtToFormat)
            {
                return FormatShortDate_WithTime("{0} {1}", dtToFormat);
            }

            public static string FormatShortDate_NoTime(DateTime dtToFormat)
            {
                return FormatShortDate_NoTime("{0}", dtToFormat);
            }

            public static string Format_DatePicker(DateTime dtToFormat)
            {
                return FormatForDatePicker(dtToFormat);
            }
            #endregion

            #region Private Functions
            private static string FormatFullDate_WithTime(String DateFormat, DateTime dtToFormat)
            {
                return FormatDateTime(dtToFormat, DateFormat, new string[] { dtToFormat.ToString(Thread.CurrentThread.CurrentCulture.DateTimeFormat.LongDatePattern), dtToFormat.ToString(Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortTimePattern) });
            }

            private static string FormatFullDate_NoTime(String DateFormat, DateTime dtToFormat)
            {
                return FormatDateTime(dtToFormat, DateFormat, new string[] { dtToFormat.ToString(Thread.CurrentThread.CurrentCulture.DateTimeFormat.LongDatePattern) });
            }

            private static string FormatShortDate_WithTime(String DateFormat, DateTime dtToFormat)
            {
                //First replace the 4 d's with 3 d's so the day is abbreviated e.g. Wednesday to Web, and 4 M's with 3 M's so the month is abbreviated e.g. Feburary to Feb
                return FormatDateTime(dtToFormat, DateFormat, new string[] { dtToFormat.ToString(FormatDateAbbrivation(Thread.CurrentThread.CurrentCulture.DateTimeFormat.LongDatePattern)), dtToFormat.ToString(Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortTimePattern) });
            }

            private static string FormatShortDate_NoTime(String DateFormat, DateTime dtToFormat)
            {
                //First replace the 4 d's with 3 d's so the day is abbreviated e.g. Wednesday to Web, and 4 M's with 3 M's so the month is abbreviated e.g. Feburary to Feb
                return FormatDateTime(dtToFormat, DateFormat, new string[] { dtToFormat.ToString(FormatDateAbbrivation(Thread.CurrentThread.CurrentCulture.DateTimeFormat.LongDatePattern)) });
            }

            private static string FormatDateTime(DateTime DateToFormat, string DateFormat, string[] DateParts)
            {
                //If the Date To Format is Today, we replace the 1st part of the DateParts with 'Today'
                //Else If the Date To Format was Yesterday, we replace the 1st part of the DateParts with 'Yesterday'
                //Else If the Date To Format is Tomorrow, we replace the 1st part of the DateParts with 'Tomorrow'
                DateTime dtCurrentDate = DateTime.Now.Date;
                if (DateToFormat.Date == dtCurrentDate)
                {
                    DateParts[0] = string.Format("{0}{1}", "Today", (DateParts.Length > 1 ? " at" : string.Empty));
                }
                else if (DateToFormat.Date == dtCurrentDate.AddDays(-1))
                {
                    DateParts[0] = string.Format("{0}{1}", "Yesterday", (DateParts.Length > 1 ? " at" : string.Empty));
                }
                else if (DateToFormat.Date == dtCurrentDate.AddDays(1))
                {
                    DateParts[0] = string.Format("{0}{1}", "Tomorrow", (DateParts.Length > 1 ? " at" : string.Empty));
                }
                return String.Format(DateFormat, DateParts);
            }

            private static string FormatDateAbbrivation(string DateFormat)
            {
                return Regex.Replace(Regex.Replace(DateFormat, "[M]{4}", "MMM"), "[d]{4}", "ddd");
            }

            private static string FormatForDatePicker(DateTime dtToFormat)
            {
                //Example format: "2016/10/12 17:00"
                return dtToFormat.ToString("yyyy/MM/dd HH:mm");
            }
            #endregion

        }

        #endregion
    }
}
