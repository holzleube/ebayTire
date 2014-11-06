using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EbaySeller.Model.Source.CSV.Extractors.Helper
{
    public interface IPlaceholderReplacer
    {
        
        string Replace(string template);
    }
}
