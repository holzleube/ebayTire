using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EbaySeller.Model.Source.CSV.Extractors;
using EbaySeller.Model.Source.CSV.Line;
using NUnit.Framework;

namespace EbaySeller.Model.Tests.CSV
{
    [TestFixture]
    public class PrestashopCsvLineTests
    {
        private ICSVLine csvLine;

        private string test =
            "ID;Active;Name;Category;MargedPrice;tax;original;0;" +
            "Discount amount;Discount percent;Discount from (yyyy-mm-dd);Discount to (yyyy-mm-dd);" +
            "Artikel Nr. #;Supplier reference #;Supplier;" +
            "Manufacturer;" +
            "EAN13;UPC;Ecotax;Width;Height;Depth;Weight;" +
            "Quantity;Minimal quantity;" +
            "Visibility;Additional shipping cost;Unity;Unit price;" +
            "Short description;Description;Tags;" +
            "Meta title;Meta keywords;Meta description;URL rewritten;Text when in stock;Text when backorder allowed;Available for order (0 = No, 1 = Yes);Product available date;Product creation date;" +
            "Show price (0 = No, 1 = Yes);Image URLs (x,y,z...);" +
            "Delete existing images (0 = No, 1 = Yes);Feature(Name:Value:Position);Available online only (0 = No, 1 = Yes);Condition;Customizable (0 = No, 1 = Yes);Uploadable files (0 = No, 1 = Yes);Text fields (0 = No, 1 = Yes);Out of stock;ID / Name of shop;Advanced stock management;Depends On Stock;Warehouse";
        
        private const string expectedLine = "ArticleId;1;ArticleName;ArticleCategory;ArticleMargePrice;53;ArticleOriginalPrice;0;" +
                                            ";;;;" +
                                            ";;;" +
                                            "ArticleManufactorer;" +
                                            ";;;;;;;" +
                                            "70;1;" +
                                            ";;;;" +
                                            "ArticleShortDescription;ArticleDescription;ArticleTags;" +
                                            ";;;;;;1;;;" +
                                            "1;ArticleImages;0;" +
                                            "ArticleFeatures;0;new;0;0;0;0;0;0;0;0";
        [SetUp]
        public void CreateCsvLine()
        {
            var dummyExtractor = GetDummyExtractor();
            csvLine = new PrestashopCSVLine(dummyExtractor);
        }

        [Test]
        public void TestSingleLine()
        {
            var testValue = csvLine.GetCSVLine("");
            Console.WriteLine(testValue);
            Console.WriteLine(expectedLine);
            Assert.AreEqual(expectedLine, testValue);
        }

        private IPropertyExtractor GetDummyExtractor()
        {
            return new DummyPropertyExtractor();
        }

    }

    internal class DummyPropertyExtractor : IPropertyExtractor
    {
        public string GetArticleId()
        {
            return "ArticleId";
        }

        public string GetArticleName()
        {
            return "ArticleName";
        }

        public string GetArticleOriginalPrice()
        {
            return "ArticleOriginalPrice";
        }

        public string GetMargedPrice()
        {
            return "ArticleMargePrice";
        }

        public string GetManufactorer()
        {
            return "ArticleManufactorer";
        }

        public string GetCategory()
        {
            return "ArticleCategory";
        }

        public string GetImageUrls()
        {
            return "ArticleImages";
        }

        public int GetAvailability()
        {
            return 70;
        }

        public string GetShortDescription(string template)
        {
            return "ArticleShortDescription";
        }

        public string GetDescription()
        {
            return "ArticleDescription";
        }

        public string GetArticleTags()
        {
            return "ArticleTags";
        }

        public string GetArticleFeatures()
        {
            return "ArticleFeatures";
        }
    }
}
