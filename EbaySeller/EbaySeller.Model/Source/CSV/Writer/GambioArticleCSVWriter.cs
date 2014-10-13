using System;
using System.Configuration;
using System.IO;
using EbaySeller.Common.DataInterface;
using EbaySeller.Model.Source.CSV.Constants;

namespace EbaySeller.Model.Source.CSV.Writer
{
    public class GambioArticleCSVWriter:AArticleCSVWriter
    {
        private string descriptionTemplate;
        private double priceMarge;

        public GambioArticleCSVWriter(string filename, double priceMarge) : base(filename, CSVConstants.FirstLineOfGambioCSV)
        {
            string filePath = ConfigurationManager.AppSettings["Gambio.Description"];
            descriptionTemplate = File.ReadAllText(filePath);
            this.priceMarge = priceMarge;
        }

        protected override string GetTextLineFromArticle(IArticle articleToWrite)
        {
            return string.Format(CSVConstants.GambioFormatLine,
                                 articleToWrite.Id, articleToWrite.ArticleId, articleToWrite.Availability,
                                 GetArticlePriceFormat(articleToWrite), articleToWrite.Manufactorer, DateTime.Now,
                                 GetImageLink(articleToWrite), "", "", 
                                 "", "", "product-" + articleToWrite.Id, 
                                 GetNameFromArticle(articleToWrite), GetDescriptionFromArticle(articleToWrite), articleToWrite.Description, 
                                 articleToWrite.Description2, GetCategoryFromArticle(articleToWrite), articleToWrite.Description,
                                 GetCategoryFromArticle(articleToWrite));
        }

        private string GetArticlePriceFormat(IArticle articleToWrite)
        {
            double calculatedPrice = articleToWrite.Price + priceMarge;
            return String.Format("{0:0.00}", calculatedPrice);
        }

        private static string GetImageLink(IArticle articleToWrite)
        {
            return articleToWrite.ArticleId.Replace(" ", string.Empty) + ".jpg";
        }

        private string GetCategoryFromArticle(IArticle articleToWrite)
        {
            IWheel wheel = (IWheel) articleToWrite;
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

        private string GetDescriptionFromArticle(IArticle articleToWrite)
        {
            IWheel article = (IWheel) articleToWrite;
            if (article == null)
            {
                return articleToWrite.Description;
            }
            return string.Format(descriptionTemplate,
                article.TyreLabelLink,
                article.Manufactorer, 
                article.WheelWidth, 
                article.WheelHeight, 
                article.CrossSection,
                article.WeightIndex, 
                article.SpeedIndex);
        }

        private string GetNameFromArticle(IArticle articleToWrite)
        {
            //IWheel wheel = (IWheel)articleToWrite;
            //if (wheel == null)
            //{
            //    return articleToWrite.Description;
            //}
            return articleToWrite.Description;
        }
    }
}
