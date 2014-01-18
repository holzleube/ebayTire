using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EbaySeller.Common.DataInterface;
using EbaySeller.Model.Source.CSV.WheelOperations.Interfaces;

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
