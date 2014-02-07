using System;
using System.Collections.Generic;
using System.Globalization;
using EbaySeller.Common.DataInterface;
using EbaySeller.Model.Source.CSV.WheelOperations;
using EbaySeller.Model.Source.Data;
using EbaySeller.Model.Source.Ebay;
using log4net;

namespace EbaySeller.Model.Source.CSV
{
    public class CSVTextHelper
    {
        private static ILog logger = LogManager.GetLogger(typeof(EbayUploader));

        public static IArticle GetArticleFromString(string textLine, int articleId)
        {
            var values = textLine.Split('|');
            IArticle article = new Article();
            var description = GetString(values[2]);
            var description2 = GetString(values[3]);
            var manufactorer = GetString(values[14]);
            if (description.Contains("DOT") || description2.Contains("DOT") || manufactorer.Equals("Syron", StringComparison.InvariantCultureIgnoreCase) || description2.Contains("DEMO"))
            {
                return null;
            }
            var tyreLabel = GetString(values[16]);
            if (IsCarWheel(description, manufactorer, tyreLabel))
            {
                try
                {
                    article = GetWheel(description, description2);
                }
                catch (Exception exception)
                {
                    logger.Warn("Exception beim Auslesen eines Autoreifens", exception);
                    return null;
                }
            }
            else
            {
                return null;
            }
            article.Id = articleId;
            article.ArticleId = GetString(values[1]);
            article.Description = description;
            article.Description2 = description2;
            article.Price = GetDouble(values[4]);
            article.Price4 = GetDouble(values[5]);
            article.AvgPrice = GetDouble(values[6]);
            article.AnonymPrice = GetDouble(values[7]);
            article.RvoPrice = GetDouble(values[8]);
            article.Availability = Convert.ToInt32(values[9]);
            article.ManufactorerNumber = GetString(values[10]);
            article.ImageLink = values[11];
            article.ImageTnLink = values[12];
            article.InfoLink = values[13];
            article.Manufactorer = manufactorer;
            article.DirectLink = values[15];
            article.TyreLabelLink = values[16];
            article.EbayIds = new Dictionary<int, string>();
            if (values.Length > 16)
            {
                article = CheckIfIdIsNotEmptyAndSetId(values[17], 1, article);
            }
            if (values.Length == 21)
            {
                article = CheckIfIdIsNotEmptyAndSetId(values[18], 2, article);
                article = CheckIfIdIsNotEmptyAndSetId(values[19], 4, article);
            }
            return article;
        }

        private static IArticle CheckIfIdIsNotEmptyAndSetId(string ebayId, int ebayKey, IArticle article)
        {
            if (!string.IsNullOrEmpty(ebayId))
            {
                article.EbayIds[ebayKey] = ebayId;
            }
            return article;
        }

        private static IArticle GetWheel(string descriptionLine, string description2)
        {
            return WheelOperationHandler.GetWheelForDescription(descriptionLine, description2);
        }

        private static bool IsCarWheel(string textLine, string manufactorer, string tyrelabel)
        {
            if (tyrelabel.Contains(@"0/0/0/0/0-h300-w300.jpg"))
            {
                return false;
            }
            if (manufactorer.Contains("STAHLRAD"))
            {
                return false;
            }
            return true;

        }

        private static string GetString(string value)
        {
            var convertedValue = value.Replace("\"", "");
            return convertedValue;
        }

        private static double GetDouble(string value)
        {
            return value.Equals(String.Empty) ? 0.0 : double.Parse(value, CultureInfo.InvariantCulture);
        }
    }
}
