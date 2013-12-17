using System.Collections.Generic;
using EbaySeller.Model.Source.Data.Interfaces;

namespace EbaySeller.Model.Source.CSV.Reader
{
    public interface ICSVReader
    {
        Dictionary<string, IArticle> ReadArticlesFromFile(string filePath);
    }
}
