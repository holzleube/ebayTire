using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EbaySeller.Model.Source.CSV.Extractors
{
    public interface IPropertyExtractor
    {
        string GetArticleId();
        string GetArticleName();
        string GetArticleOriginalPrice();
        string GetMargedPrice();
        string GetManufactorer();
        string GetCategory();
        string GetImageUrls();
        int GetAvailability();
        string GetShortDescription();
        string GetDescription();
        string GetArticleTags();
        string GetArticleFeatures();
    }
}
