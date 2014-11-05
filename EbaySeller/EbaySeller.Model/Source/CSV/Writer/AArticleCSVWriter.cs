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
        protected double priceMarge;

        protected AArticleCSVWriter(string filename, string firstLine, double priceMarge)
        {
            this.fileName = filename;
            this.originalName = fileName;
            this.priceMarge = priceMarge;
            this.firstLine = firstLine;
            //WriteTextToFile(firstLine, false);
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

        public abstract string GetTextLineFromArticle(IArticle articleToWrite);

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

        protected string GetArticlePriceFormat(IArticle articleToWrite)
        {
            double calculatedPrice = articleToWrite.Price + priceMarge;
            return calculatedPrice.ToString("N");
            //return String.Format("{0:0.00}", calculatedPrice);
        }

        protected string GetNameFromArticle(IArticle articleToWrite)
        {
            //IWheel wheel = (IWheel)articleToWrite;
            //if (wheel == null)
            //{
            //    return articleToWrite.Description;
            //}
            return articleToWrite.Description;
        }

        protected string GetCategoryFromArticle(IArticle articleToWrite)
        {
            IWheel wheel = (IWheel)articleToWrite;
            if (wheel == null)
            {
                return "Sonstiges";
            }
            if (wheel.IsWinter)
            {
                return "Winterreifen";
            }
            if (wheel.IsMudSnow)
            {
                return "Ganzjahresreifen";
            }
            return "Sommerreifen";
        }

        protected string GetDescriptionFromArticle(IArticle articleToWrite)
        {
            IWheel article = (IWheel)articleToWrite;
            if (article == null)
            {
                return articleToWrite.Description;
            }
            return "Beschreibung";
            //string.Format(descriptionTemplate,
            //    article.TyreLabelLink,
            //    article.Manufactorer,
            //    article.WheelWidth,
            //    article.WheelHeight,
            //    article.CrossSection,
            //    article.WeightIndex,
            //    article.SpeedIndex);
        }

    }
}
