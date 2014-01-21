using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EbaySeller.Common.DataInterface;
using EbaySeller.Model.Source.Exceptions;
using log4net;

namespace EbaySeller.Model.Source.CSV.Reader
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
                        if (article == null)
                        {
                            continue;
                        }
                        var key = article.Description + article.Description2;
                        if (result.ContainsKey(key))
                        {
                            IWheel wheel = article as IWheel;
                            if (wheel == null)
                            {
                                continue;
                            }
                            
                            result.Remove(key);
                            result.Add(key, article);
                        }
                        result.Add(key, article);
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
