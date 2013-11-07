using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EbaySeller.Model.Source.Data.Interfaces;
using EbaySeller.Model.Source.Exceptions;

namespace EbaySeller.Model.Source.CSV
{
    public class CSVReader : ICSVReader
    {
        public List<IArticle> ReadArticlesFromFile(string filePath)
        {
            try
            {
                var textLines = File.ReadAllLines(filePath);
                return textLines.Skip(1).Select(CSVTextHelper.GetArticleFromString).ToList();
            }
            catch (IOException exception)
            {
                throw new FileNotReadyException();
            }
            
        }
    }
}
