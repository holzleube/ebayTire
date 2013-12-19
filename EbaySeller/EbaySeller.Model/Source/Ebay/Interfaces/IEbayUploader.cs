using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EbaySeller.Model.Source.Data.Interfaces;

namespace EbaySeller.Model.Source.Ebay.Interfaces
{
    public interface IEbayUploader
    {
        List<IArticle> RefreshOrCreateEbayArticle(List<IArticle> articlesToRefresh, string filename, double percentage, double amount);
    }
}
