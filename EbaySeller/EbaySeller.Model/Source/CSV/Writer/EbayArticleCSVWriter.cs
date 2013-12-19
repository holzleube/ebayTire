using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using EbaySeller.Model.Source.CSV.Constants;
using EbaySeller.Model.Source.Data.Interfaces;
using log4net;

namespace EbaySeller.Model.Source.CSV.Writer
{
    public class EbayArticleCSVWriter:ICSVWriter
    {
        private string fileName;
        private ILog logger = LogManager.GetLogger(typeof(EbayArticleCSVWriter));

        public EbayArticleCSVWriter(string filename)
        {
            this.fileName = filename;
            WriteTextToFile(CSVConstants.FirstLineOfCsvFile, false);
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

        private void WriteTextToFile(string csvTextLine, bool append)
        {
            using (var file = new StreamWriter(@fileName, append))
            {
                file.WriteLine(csvTextLine);
            }
        }

        private string GetTextLineFromArticle(IArticle articleToWrite)
        {
            return string.Format(CSVConstants.DataFormatLine, 
                articleToWrite.Id,
                articleToWrite.ArticleId,
                articleToWrite.Description,
                articleToWrite.Description2,
                articleToWrite.Price.ToString("0.00", CultureInfo.InvariantCulture),
                articleToWrite.Price4.ToString("0.00", CultureInfo.InvariantCulture),
                articleToWrite.AvgPrice.ToString("0.00", CultureInfo.InvariantCulture),
                articleToWrite.AnonymPrice.ToString("0.00", CultureInfo.InvariantCulture),
                articleToWrite.RvoPrice.ToString("0.00", CultureInfo.InvariantCulture),
                articleToWrite.Availability,
                articleToWrite.ManufactorerNumber,
                articleToWrite.ImageLink,
                articleToWrite.ImageTnLink,
                articleToWrite.InfoLink,
                articleToWrite.Manufactorer,
                articleToWrite.DirectLink,
                articleToWrite.TyreLabelLink,
                articleToWrite.EbayId,
                DateTime.Now);
        }
    }
}
