using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EbaySeller.Model.Source.Data.Interfaces;

namespace EbaySeller.Model.Source.CSV
{
    public interface ICSVReader
    {
        Dictionary<string, IArticle> ReadArticlesFromFile(string filePath);
    }
}
