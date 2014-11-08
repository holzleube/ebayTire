using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EbaySeller.Common.DataInterface;
using EbaySeller.Model.Source.CSV.Extractors;
using EbaySeller.Model.Source.Data;
using NUnit.Framework;

namespace EbaySeller.Model.Tests.CSV
{
    [TestFixture]
    public class ArticlePropertyExtractorTests
    {
        private IArticlePropertyExtractorFactory articlePropertyExtractorFactory;

        [SetUp]
        public void Setup()
        {
            this.articlePropertyExtractorFactory = new ArticlePropertyExtractorFactory(2, "");
        }

        [Test]
        public void TestCreationOfArticleExtractor()
        {
            var result = articlePropertyExtractorFactory.GetPropertyExtractor(new Article());
            Assert.IsTrue(result is PrestoshopPropertyExtractor);
        }

        [Test]
        public void TestCreationOfWheelExtractor()
        {
            IArticle article = new Wheel();
            var result = articlePropertyExtractorFactory.GetPropertyExtractor(article);
            Assert.IsTrue(result is PrestoshopWheelPropertyExtractor);
        }
    }
}
