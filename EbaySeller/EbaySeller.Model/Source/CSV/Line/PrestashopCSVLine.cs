using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EbaySeller.Model.Source.CSV.Constants;
using EbaySeller.Model.Source.CSV.Extractors;

namespace EbaySeller.Model.Source.CSV.Line
{
    public class PrestashopCSVLine:ICSVLine
    {
        private IPropertyExtractor propertyExtractor;

        public PrestashopCSVLine(IPropertyExtractor propertyExtractor)
        {
            this.propertyExtractor = propertyExtractor;
        }

        public string GetCSVLine()
        {
            return String.Format(CSVConstants.PrestoFormatLine,
                propertyExtractor.GetArticleId(),
                propertyExtractor.GetArticleName(),
                propertyExtractor.GetCategory(),
                propertyExtractor.GetMargedPrice(),
                propertyExtractor.GetArticleOriginalPrice(),
                propertyExtractor.GetManufactorer(),
                propertyExtractor.GetAvailability(),
                propertyExtractor.GetShortDescription(),
                propertyExtractor.GetDescription(),
                propertyExtractor.GetArticleTags(),
                propertyExtractor.GetImageUrls(),
                propertyExtractor.GetArticleFeatures()
                );
        }
    }
}
