using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EbaySeller.Common.DataInterface;

namespace EbaySeller.Model.Source.CSV.Extractors.Helper
{
    public class PlaceholderReplacer:IPlaceholderReplacer
    {
        protected IDictionary<string, string> replaceMap;

        public PlaceholderReplacer(IArticle article)
        {
            InitReplaceMap(article);
        }

        private void InitReplaceMap(IArticle article)
        {
            this.replaceMap = new Dictionary<string, string>();
            replaceMap.Add(Placeholder.ArticlePlaceholder.NamePlaceholder, article.Description);
            replaceMap.Add(Placeholder.ArticlePlaceholder.DescriptionPlaceholder,article.Description + " " + article.Description2);
            replaceMap.Add(Placeholder.ArticlePlaceholder.ManufactorPlaceholder, article.Manufactorer);
        }

        public string Replace(string template)
        {
            return replaceMap.Keys.Aggregate(template, (current, placeholder) => current.Replace(placeholder, replaceMap[placeholder]));
        }
    }
}
