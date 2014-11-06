using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EbaySeller.Common.DataInterface;
using EbaySeller.Model.Source.CSV.Extractors;

namespace EbaySeller.Model.Source.CSV.Writer
{
    public class ArticleCsvWriter:AArticleCSVWriter
    {
        public ArticleCsvWriter(string filename, double priceMarge, IArticlePropertyExtractorFactory factory) : base(filename, "", priceMarge)
        {
        }

        public override string GetTextLineFromArticle(IArticle articleToWrite)
        {
            throw new NotImplementedException();
        }
    }
}
