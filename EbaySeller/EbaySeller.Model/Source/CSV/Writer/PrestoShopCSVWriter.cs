using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EbaySeller.Common.DataInterface;
using EbaySeller.Model.Source.CSV.Constants;

namespace EbaySeller.Model.Source.CSV.Writer
{
    public class PrestoShopCSVWriter : AArticleCSVWriter
    {
        public PrestoShopCSVWriter(string filename, double priceMarge)
            : base(filename, CSVConstants.PrestoFormatLine, priceMarge)
        {
        }

        public  override string GetTextLineFromArticle(IArticle articleToWrite)
        {
            return String.Format(CSVConstants.PrestoFormatLine,
                articleToWrite.ArticleId,
                GetNameFromArticle(articleToWrite),
                GetCategoryFromArticle(articleToWrite),
                GetArticlePriceFormat(articleToWrite),
                20,
                articleToWrite.Manufactorer,
                articleToWrite.Availability,
                GetShortDescription(articleToWrite),
                GetDescriptionFromArticle(articleToWrite),
                GetTagsFromArticle(articleToWrite),
                GetImageUrls(articleToWrite) 
                );
        }

        private string GetImageUrls(IArticle articleToWrite)
        {
            return articleToWrite.ImageLink;
        }

        private string GetTagsFromArticle(IArticle articleToWrite)
        {
            return "Tags";
        }

        private string GetShortDescription(IArticle articleToWrite)
        {
            return "kurze Beschreibung";
        }
    }
}
