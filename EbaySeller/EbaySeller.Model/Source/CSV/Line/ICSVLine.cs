using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EbaySeller.Model.Source.CSV.Line
{
    public interface ICSVLine
    {
        string GetCSVLine();
    }
}
