using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EbaySeller.Model.Source.CSV.WheelOperations.Interfaces;
using EbaySeller.Model.Source.Data.Interfaces;

namespace EbaySeller.Model.Source.CSV.WheelOperations
{
    public class WheelDotOperation:IWheelOperation
    {
        public IWheel SetValueOnWheel(IWheel wheel, string pattern)
        {
            var  year = pattern.Trim(' ').Replace("DOT", " ");
            int convertedYear = AWheelOperations.GetIntegerFromString(year);
            wheel.DotNumber = convertedYear;
            return wheel;
        }

        public string GetRegexPattern()
        {
            return @"DOT ?20[\d]{2}";
        }
    }
}
