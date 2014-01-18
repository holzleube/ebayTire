using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EbaySeller.Common.DataInterface;
using EbaySeller.Model.Source.CSV.WheelOperations.Interfaces;

namespace EbaySeller.Model.Source.CSV.WheelOperations
{
    public class WheelHeightWidthOperation : IWheelOperation
    {
        public IWheel SetValueOnWheel(IWheel wheel, string pattern)
        {
            var splittet = pattern.Split('/');
            wheel.WheelWidth = AWheelOperations.GetIntegerFromString(splittet[0]);
            wheel.WheelHeight = AWheelOperations.GetIntegerFromString(splittet[1]);
            return wheel;
        }

        public string GetRegexPattern()
        {
            return @"\d{3}/\d{2}";
        }
    }
}
