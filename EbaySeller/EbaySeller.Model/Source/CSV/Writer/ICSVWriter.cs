using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EbaySeller.Model.Source.Data.Interfaces;

namespace EbaySeller.Model.Source.CSV.Writer
{
    public interface ICSVWriter
    {
        void WriteToCSVFile(IArticle articleToWrite);
    }
}
