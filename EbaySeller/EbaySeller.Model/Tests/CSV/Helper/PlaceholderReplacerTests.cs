using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EbaySeller.Common.DataInterface;
using EbaySeller.Model.Source.CSV.Extractors.Helper;
using EbaySeller.Model.Source.Data;
using NUnit.Framework;

namespace EbaySeller.Model.Tests.CSV.Helper
{
    public class PlaceholderReplacerTests
    {
        private IPlaceholderReplacer replacer;
        private IArticle testArticle;

        [SetUp]
        public void CreateReplacer()
        {
            testArticle = GetTestArticle();
            replacer = new PlaceholderReplacer(testArticle);
        }

        private IArticle GetTestArticle()
        {
            return new Article()
                {
                    Description = "testDescription",
                    Description2 = "secondDescription",
                    Manufactorer = "manufactorer"
                };
        }

        [Test]
        public void TestNameReplacement()
        {
            var template = Placeholder.ArticlePlaceholder.NamePlaceholder;
            var result = replacer.Replace(template);
            Assert.AreEqual(testArticle.Description, result);
        }

        [Test]
        public void TestNameDescriptionReplacement()
        {
            var result = replacer.Replace(Placeholder.ArticlePlaceholder.DescriptionPlaceholder);
            Assert.AreEqual(testArticle.Description + " " + testArticle.Description2, result);
        }

        [Test]
        public void TestManufactorerReplacement()
        {
            var template = Placeholder.ArticlePlaceholder.ManufactorPlaceholder;
            var result = replacer.Replace(template);
            Assert.AreEqual(testArticle.Manufactorer, result);
        }

    }
}
