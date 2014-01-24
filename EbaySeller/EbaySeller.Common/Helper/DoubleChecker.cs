using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EbaySeller.Common.Helper
{
    public static class DoubleChecker
    {
        public static bool IsValidDouble(string stringToCheck, out double variable)
        {
            variable = 0.0;
            if (stringToCheck.Contains("."))
            {
                return false;
            }
            if (stringToCheck.Equals(string.Empty))
            {
                return false;
            }
            try
            {
                variable = double.Parse(stringToCheck);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
