using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EbaySeller.Model.Source.CSV.Writer;
using EbaySeller.Model.Source.Data.Interfaces;

namespace EbaySeller.Model.Source.Ebay.Interfaces
{
    public interface IEbayUploader
    {
        IArticle RefreshOrCreateEbayArticle(IArticle articleToRefresh, EbayArticleCSVWriter csvWriter, double amount, string template);
    }
}
