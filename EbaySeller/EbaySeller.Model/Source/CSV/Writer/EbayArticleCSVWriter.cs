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
    public class EbayArticleCSVWriter:AArticleCSVWriter
    {
        public EbayArticleCSVWriter(string filename) : base(filename, CSVConstants.FirstLineOfCsvFile)
        {
        }

        protected override string GetTextLineFromArticle(IArticle articleToWrite)
        {
            return string.Format(CSVConstants.DataFormatLine, 
                articleToWrite.Id,
                articleToWrite.ArticleId,
                articleToWrite.Description,
                articleToWrite.Description2,
                GetNumberFormatForDecimal(articleToWrite.Price),
                GetNumberFormatForDecimal(articleToWrite.Price4),
                GetNumberFormatForDecimal(articleToWrite.AvgPrice),
                GetNumberFormatForDecimal(articleToWrite.AnonymPrice),
                GetNumberFormatForDecimal(articleToWrite.RvoPrice),
                articleToWrite.Availability,
                articleToWrite.ManufactorerNumber,
                articleToWrite.ImageLink,
                articleToWrite.ImageTnLink,
                articleToWrite.InfoLink,
                articleToWrite.Manufactorer,
                articleToWrite.DirectLink,
                articleToWrite.TyreLabelLink,
                GetEbayIdForArticle(articleToWrite, 1),
                GetEbayIdForArticle(articleToWrite, 2),
                GetEbayIdForArticle(articleToWrite, 4),
                DateTime.Now);
        }

        

        private static string GetEbayIdForArticle(IArticle articleToWrite, int key)
        {
            if (articleToWrite.EbayIds.ContainsKey(key))
            {
                return articleToWrite.EbayIds[key];
            }
            return "";
        }
    }
}
