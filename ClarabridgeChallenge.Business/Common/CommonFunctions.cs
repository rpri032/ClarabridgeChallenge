using System;
using System.Configuration;
using System.Text.RegularExpressions;

namespace ClarabridgeChallenge.Business.Common
{
    public class CommonFunctions
    {
        public static string GetApplicationSetting(string Key, string defaultValue)
        {
            var applicationSettingValue = defaultValue;
            if (ConfigurationManager.AppSettings[Key] != null)
            {
                applicationSettingValue = ConfigurationManager.AppSettings[Key];
            }
            return applicationSettingValue.ToLower();
        }

        public static string RemoveHtml(string htmlString)
        {
            return RemoveHtml(htmlString, Environment.NewLine);
        }

        public static string RemoveHtml(string htmlString, string breakCharacterReplacement)
        {
            string sCleanText = Regex.Replace(htmlString, @"&nbsp;", " ");
            sCleanText = Regex.Replace(sCleanText, @"\s*<br(\s)*/>\s*", breakCharacterReplacement);
            sCleanText = Regex.Replace(sCleanText, "<(.|\n)*?>", string.Empty);
            return sCleanText;
        }
    }
}
