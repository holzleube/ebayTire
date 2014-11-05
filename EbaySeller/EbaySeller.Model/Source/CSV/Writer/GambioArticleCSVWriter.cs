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
        

        public GambioArticleCSVWriter(string filename, double priceMarge) : base(filename, CSVConstants.FirstLineOfGambioCSV, priceMarge)
        {
            string filePath = ConfigurationManager.AppSettings["Gambio.Description"];
            descriptionTemplate = File.ReadAllText(filePath);
            this.priceMarge = priceMarge;
        }

        public override string GetTextLineFromArticle(IArticle articleToWrite)
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

       

        private static string GetImageLink(IArticle articleToWrite)
        {
            return articleToWrite.ArticleId.Replace(" ", string.Empty) + ".jpg";
        }

        

       
    }
}
