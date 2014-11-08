using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EbaySeller.Common.DataInterface;
using EbaySeller.Model.Source.CSV.Constants;
using EbaySeller.Model.Source.CSV.Extractors;
using EbaySeller.Model.Source.Data;
using NUnit.Framework;

namespace EbaySeller.Model.Tests.CSV
{
    [TestFixture]
    public class PrestoshopPropertyExtractorTests
    {
        IPropertyExtractor extractor;
        private IArticle testArticle;

        [SetUp]
        public void InitTestClass()
        {
           testArticle = GetTestArticle();
           InitExtractor();
        }

        [Test]
        public void TestIdOfArticle()
        {
            var testValue = extractor.GetArticleId();
            var description1 = testArticle.Description;
            var description2 = testArticle.Description2;
            Assert.AreEqual(description1.Trim()+description2.Trim(), testValue);
        }

        [Test]
        public void TestIdOfArticleWithNullValues()
        {
            testArticle.Description2 = null;
            extractor = new PrestoshopPropertyExtractor(testArticle, 2);
            var testValue = extractor.GetArticleId();
            Assert.AreEqual(testArticle.Description, testValue);
        }

        [Test]
        public void TestArticleName()
        {
            var testValue = extractor.GetArticleName();
            Assert.AreEqual(testArticle.Description, testValue);
        }

        [Test]
        public void TestOriginalPriceOfArticle()
        {
            var testValue = extractor.GetArticleOriginalPrice();
            Assert.AreEqual("98,00", testValue);
        }

        [Test]
        public void TestMargePrice()
        {
            var testValue = extractor.GetMargedPrice();
            Assert.AreEqual("100,00", testValue);
        }

        [Test]
        public void TestManufactorer()
        {
            var testValue = extractor.GetManufactorer();
            Assert.AreEqual(testArticle.Manufactorer, testValue);
        }

        [Test]
        public void TestCategory()
        {
            var testValue = extractor.GetCategory();
            Assert.AreEqual(CSVConstants.DefaultCategoryName, testValue);
        }

        [Test]
        public void TestSingleImageUrl()
        {
            testArticle.InfoLink = "";
            InitExtractor();
            var testValue = extractor.GetImageUrls();
            Assert.AreEqual(testArticle.ImageLink, testValue);
        }

        [Test]
        public void TestImageUrls()
        {
            var testValue = extractor.GetImageUrls();
            Assert.AreEqual(string.Format("{0},{1}", testArticle.ImageLink, testArticle.InfoLink), testValue);
        }

        [Test]
        public void TestEmptyImageUrls()
        {
            testArticle.InfoLink = "";
            testArticle.ImageLink = "";
            InitExtractor();
            var testValue = extractor.GetImageUrls();
            Assert.AreEqual("", testValue);
        }

        [Test]
        public void TestAvailabilityMoreThan30()
        {
            var testValue = extractor.GetAvailability();
            Assert.AreEqual(30, testValue);
        }
        
        [Test]
        public void TestAvailabilityLessThan20()
        {
            testArticle.Availability = 7;
            InitExtractor();
            var testValue = extractor.GetAvailability();
            Assert.AreEqual(7, testValue);
        } 

        [Test]
        public void TestShortDescription()
        {
            var testValue = extractor.GetShortDescription();
            Assert.AreEqual(testArticle.Description, testValue);
        }

        [Test]
        public void TestCompleteDescription()
        {
            var testValue = extractor.GetDescription();
            Assert.AreEqual(testArticle.Description + " " + testArticle.Description2, testValue);
        }

        [Test]
        public void TestArticleTags()
        {
            string testValue = extractor.GetArticleTags();
            Assert.AreEqual(CSVConstants.DefaultCategoryName, testValue);
        }

        [Test]
        public void TestArticleFeatures()
        {
            var testValue = extractor.GetArticleFeatures();
            Assert.AreEqual("", testValue);
        }
        
        private void InitExtractor()
        {
            extractor = new PrestoshopPropertyExtractor(testArticle, 2);
        }

        private IArticle GetTestArticle()
        {
            return new Article()
            {
                Id = 1,
                AnonymPrice = 1,
                Description = "YOKOHAMA S-DRI  255/40 R17",
                Description2 = "DOT 2011",
                Availability = 70,
                Price = 98,
                Manufactorer = "YOKOHAMA",
                ArticleId = "S2554017ZYOSDRIVE",
                ImageLink = "http://media2.tyre24.de/images/tyre/3052-NTI2NzY=-w300-h300-br1.jpg",
                InfoLink = "http://media2.tyre24.de/tyrelabel/8/5/2/73/2-h300-w300.jpg",
            };
        }
    }
}
