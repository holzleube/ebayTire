using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EbaySeller.Common.DataInterface;

namespace EbaySeller.Common.Helper
{
    public static class ArticleKeyGenerator
    {
        public static string GetKeyFromArticle(IArticle article)
        {
            return article.Description + article.Description2;
        }
    }
}
