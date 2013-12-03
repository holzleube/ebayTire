using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EbaySeller.Model.Source.CSV.WheelOperations
{
    public class AWheelOperations
    {
        public static int GetIntegerFromString(string pattern)
        {
            try
            {
                return int.Parse(pattern);
            }
            catch (Exception)
            {
                return 0;
            }

        }

        public static string GetPattern(string descriptionLine, string regexToFind)
        {
            Regex reg = new Regex(regexToFind);
            var match = reg.Matches(descriptionLine);
            for (int index = 0; index < match.Count; index++)
            {
                return match[0].ToString();
            }
            return "";
        }

        public static string CutFromString(string originalString, string patternToRemove)
        {
            int Place = originalString.IndexOf(patternToRemove);
            string result = originalString.Remove(Place, patternToRemove.Length);
            return result;
        }

    }
}
