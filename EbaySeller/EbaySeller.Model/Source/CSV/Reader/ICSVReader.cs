using System.Collections.Generic;
using EbaySeller.Common.DataInterface;

namespace EbaySeller.Model.Source.CSV.Reader
{
    public interface ICSVReader
    {
        Dictionary<string, IArticle> ReadArticlesFromFile(string filePath);
    }
}
