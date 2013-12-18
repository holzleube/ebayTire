using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EbaySeller.Model.Source.CSV.Constants;
using EbaySeller.Model.Source.Data.Interfaces;

namespace EbaySeller.Model.Source.CSV.Writer
{
    public class EbayArticleCSVWriter:ICSVWriter
    {
        private string fileName;

        public EbayArticleCSVWriter(string filename)
        {
            this.fileName = filename;
            WriteTextToFile(CSVConstants.FirstLineOfCsvFile);
        }
        public void WriteToCSVFile(IArticle articleToWrite)
        {
            string csvTextLine = GetTextLineFromArticle(articleToWrite);
            WriteTextToFile(csvTextLine);
        }

        private void WriteTextToFile(string csvTextLine)
        {
            using (var file = new StreamWriter(@fileName, true))
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
                articleToWrite.Price,
                articleToWrite.Price4,
                articleToWrite.AvgPrice,
                articleToWrite.AnonymPrice,
                articleToWrite.RvoPrice,
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
