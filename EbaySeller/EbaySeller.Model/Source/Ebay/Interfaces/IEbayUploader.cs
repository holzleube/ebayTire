using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EbaySeller.Common.DataInterface;
using EbaySeller.Model.Source.CSV.Writer;

namespace EbaySeller.Model.Source.Ebay.Interfaces
{
    public interface IEbayUploader
    {
        IArticle RefreshOrCreateEbayArticle(IArticle articleToRefresh, EbayArticleCSVWriter csvWriter, double amount, string template);

        void RemoveAllEbayArticles(IArticle articleToDelete);
    }
}
