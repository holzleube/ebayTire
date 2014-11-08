using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EbaySeller.Common.DataInterface;
using EbaySeller.Model.Source.CSV.Constants;
using EbaySeller.Model.Source.CSV.Extractors.Helper;

namespace EbaySeller.Model.Source.CSV.Extractors
{
    public class PrestoshopPropertyExtractor:IPropertyExtractor
    {
        private IArticle article;
        protected IPlaceholderReplacer placeholderReplacer;
        private double marge;

        public PrestoshopPropertyExtractor(IArticle articleToUse, double marge):this(articleToUse, marge, new PlaceholderReplacer(articleToUse))
        {
        }

        public PrestoshopPropertyExtractor(IArticle articleToUse, double marge, IPlaceholderReplacer placeholderReplacer)
        {
            this.article = articleToUse;
            this.marge = marge;
            this.placeholderReplacer = placeholderReplacer;
        }

        public string GetArticleId()
        {
            return GetNullcheckedTrimmedString(article.Description) + GetNullcheckedTrimmedString(article.Description2);
        }

        private string GetNullcheckedTrimmedString(string valueToTrim)
        {
            return valueToTrim == null ? "" : valueToTrim.Trim();
        }

        public virtual string GetArticleName()
        {
            return article.Description;
        }

        public string GetArticleOriginalPrice()
        {
            return GetDecimalPrice(article.Price);
        }

        public string GetMargedPrice()
        {
            return GetDecimalPrice(article.Price + marge);
        }

        public string GetManufactorer()
        {
            return article.Manufactorer;
        }

        public virtual string GetCategory()
        {
            return CSVConstants.DefaultCategoryName;
        }

        public string GetImageUrls()
        {
            string[] resultList = GetNotEmptyStringList();
            return string.Join(",", resultList);
        }

        public int GetAvailability()
        {
            if (article.Availability > 29)
            {
                return 30;
            }
            return article.Availability;
        }

        public string GetShortDescription()
        {
            return ReplaceTemplate(Placeholder.ArticlePlaceholder.NamePlaceholder);
        }

        public virtual string GetDescription()
        {
            return ReplaceTemplate(Placeholder.ArticlePlaceholder.DescriptionPlaceholder);
        }

        public string GetArticleTags()
        {
            return CSVConstants.DefaultCategoryName;
        }

        public virtual string GetArticleFeatures()
        {
            return "";
        }

        private string ReplaceTemplate(string template)
        {
            return placeholderReplacer.Replace(template);
        }

        private string[] GetNotEmptyStringList()
        {
            var list = new List<string>();
            list = AddStringIfNotNull(article.ImageLink, list);
            list = AddStringIfNotNull(article.ImageTnLink, list);
            list = AddStringIfNotNull(article.InfoLink, list);
            return list.ToArray();
        }

        private List<string> AddStringIfNotNull(string link, List<string> list)
        {
            if (string.IsNullOrEmpty(link))
            {
                return list;
            }
            list.Add(link);
            return list;
        }

        private string GetDecimalPrice(double price)
        {
            return price.ToString("N");
        }
    }

}
