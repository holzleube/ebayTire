using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EbaySeller.Model.Source.Data.Interfaces;

namespace EbaySeller.Model.Source.CSV
{
    public class CSVReader : ICSVReader
    {
        public List<IArticle> ReadArticlesFromFile(string filePath)
        {
            var textLines = File.ReadAllLines(filePath);
            return textLines.Select(CSVTextHelper.GetArticleFromString).ToList();
        }
    }
}
