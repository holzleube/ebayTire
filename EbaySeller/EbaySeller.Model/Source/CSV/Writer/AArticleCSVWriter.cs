using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using EbaySeller.Common.DataInterface;
using EbaySeller.Model.Source.CSV.Constants;
using log4net;

namespace EbaySeller.Model.Source.CSV.Writer
{
    public abstract class AArticleCSVWriter:ICSVWriter
    {
        private ILog logger = LogManager.GetLogger(typeof(AArticleCSVWriter));
        private string fileName;
        private string originalName;
        private string firstLine;

        protected AArticleCSVWriter(string filename, string firstLine)
        {
            this.fileName = filename;
            this.originalName = fileName;
            this.firstLine = firstLine;
            WriteTextToFile(firstLine, false);
        }

        public void WriteToCSVFile(List<IArticle> articlesToWrite)
        {
            int counter = 0;
            foreach (var articleToWrite in articlesToWrite)
            {
                if (counter % 1000 == 0 && counter != 0)
                {
                    fileName = originalName + counter + ".csv";
                    WriteTextToFile(firstLine, false);
                }
                WriteToCSVFile(articleToWrite);
                counter++;
            }
        }

        public void WriteToCSVFile(IArticle articleToWrite)
        {
            if (articleToWrite == null)
            {
                logger.Warn("Article in Write To CSV was null");
                return;
            }
            string csvTextLine = GetTextLineFromArticle(articleToWrite);
            WriteTextToFile(csvTextLine, true);
        }

        protected abstract string GetTextLineFromArticle(IArticle articleToWrite);

        private void WriteTextToFile(string csvTextLine, bool append)
        {
            using (var file = new StreamWriter(@fileName, append))
            {
                file.WriteLine(csvTextLine);
            }
        }

        protected string GetNumberFormatForDecimal(double numberToConvert)
        {
            return numberToConvert.ToString("0.00", CultureInfo.InvariantCulture);
        }

    }
}
