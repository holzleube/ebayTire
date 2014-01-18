using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EbaySeller.Common.DataInterface;

namespace EbaySeller.Model.Source.CSV.WheelOperations.Interfaces
{
    public interface IWheelOperation
    {
        IWheel SetValueOnWheel(IWheel wheel, string pattern);

        string GetRegexPattern();
    }
}
