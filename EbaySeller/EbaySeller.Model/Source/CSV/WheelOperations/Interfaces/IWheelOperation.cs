﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EbaySeller.Model.Source.Data.Interfaces;

namespace EbaySeller.Model.Source.CSV.WheelOperations.Interfaces
{
    public interface IWheelOperation
    {
        IWheel SetValueOnWheel(IWheel wheel, string pattern);

        string GetRegexPattern();
    }
}
