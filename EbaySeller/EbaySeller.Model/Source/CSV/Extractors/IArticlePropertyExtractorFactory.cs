﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EbaySeller.Common.DataInterface;

namespace EbaySeller.Model.Source.CSV.Extractors
{
    public interface IArticlePropertyExtractorFactory
    {
        IPropertyExtractor GetPropertyExtractor(IArticle article);
        IPropertyExtractor GetPropertyExtractor(IWheel article);
    }
}
