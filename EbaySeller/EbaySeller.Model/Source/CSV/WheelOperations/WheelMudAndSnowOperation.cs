using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EbaySeller.Model.Source.CSV.WheelOperations.Interfaces;
using EbaySeller.Model.Source.Data.Interfaces;

namespace EbaySeller.Model.Source.CSV.WheelOperations
{
    public class WheelMudAndSnowOperation : IWheelOperation
    {
        public IWheel SetValueOnWheel(IWheel wheel, string pattern)
        {
            wheel.IsMudSnow = true;
            return wheel;
        }

        public string GetRegexPattern()
        {
            return @"M ?\+ ?S";
        }
    }
}
