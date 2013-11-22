using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EbaySeller.Model.Source.Data.Interfaces;
using EbaySeller.Model.Source.Exceptions;
using log4net;

namespace EbaySeller.Model.Source.CSV
{
    public class CSVReader : ICSVReader
    {

        public Dictionary<string, IArticle> ReadArticlesFromFile(string filePath)
        {
            var logger = LogManager.GetLogger(this.GetType());
            try
            {
                var textLines = File.ReadAllLines(filePath);
                var result = new Dictionary<string, IArticle>();
                int index = 1;
                foreach (var textLine in textLines.Skip(1))
                {
                    try
                    {
                        
                        var article = CSVTextHelper.GetArticleFromString(textLine, index);
                        if (result.ContainsKey(article.ArticleId))
                        {
                            continue;
                        }
                        result.Add(article.ArticleId, article);
                        index++;
                    }
                    catch (Exception e)
                    {
                        logger.Info("Couldnt read line: "+textLine, e);
                    }
                }
                return result;
            }
            catch (IOException exception)
            {
                logger.Error("Unerwarteter Fehler: ", exception);
                throw new FileNotReadyException();
            }
        }
    }
}
