using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EbaySeller.Common.DataInterface;
using EbaySeller.Model.Source.CSV.Constants;
using EbaySeller.Model.Source.CSV.Extractors;
using EbaySeller.Model.Source.CSV.Line;

namespace EbaySeller.Model.Source.CSV.Writer
{
    public class ArticleCsvWriter:AArticleCSVWriter
    {
        private IArticlePropertyExtractorFactory factory;

        public ArticleCsvWriter(string filename, IArticlePropertyExtractorFactory factory) : base(filename, CSVConstants.PrestoFirstLine, 0)
        {
            this.factory = factory;
        }

        public override string GetTextLineFromArticle(IArticle articleToWrite)
        {
            var propertyExtractor = factory.GetPropertyExtractor(articleToWrite);
            return new PrestashopCSVLine(propertyExtractor).GetCSVLine();
        }
    }
}
