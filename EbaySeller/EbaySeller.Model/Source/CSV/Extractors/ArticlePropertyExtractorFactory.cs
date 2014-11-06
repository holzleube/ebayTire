using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EbaySeller.Common.DataInterface;

namespace EbaySeller.Model.Source.CSV.Extractors
{
    public class ArticlePropertyExtractorFactory:IArticlePropertyExtractorFactory
    {
        private double marge;
        private string descriptionTemplate;

        public ArticlePropertyExtractorFactory(double marge, string descriptionTemplate)
        {
            this.marge = marge;
            this.descriptionTemplate = descriptionTemplate;
        }
        public IPropertyExtractor GetPropertyExtractor(IArticle article)
        {
            return new PrestoshopPropertyExtractor(article, marge);
        }

        public IPropertyExtractor GetPropertyExtractor(IWheel wheel)
        {
            return new PrestoshopWheelPropertyExtractor(wheel, marge, descriptionTemplate);
        }
    }
}
