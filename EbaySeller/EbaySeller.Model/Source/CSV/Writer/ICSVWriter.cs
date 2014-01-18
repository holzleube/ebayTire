using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EbaySeller.Common.DataInterface;

namespace EbaySeller.Model.Source.CSV.Writer
{
    public interface ICSVWriter
    {
        void WriteToCSVFile(IArticle articleToWrite);
    }
}
