using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EbaySeller.Common.DataInterface;
using EbaySeller.Model.Source.CSV.Constants;
using EbaySeller.Model.Source.CSV.Extractors;
using EbaySeller.Model.Source.CSV.Extractors.Helper;
using EbaySeller.Model.Source.Data;
using NUnit.Framework;

namespace EbaySeller.Model.Tests.CSV
{
    [TestFixture]
    public class PrestashopPropertyWheelExtractorTests
    {
        private IWheel wheelToTest;
        private IPropertyExtractor extractor;
        private string testValue;

        [SetUp]
        public void SetupWheelPropertyExtractor()
        {
            testValue = "";
            wheelToTest = GetWheelToTest();
            extractor = new PrestoshopWheelPropertyExtractor(wheelToTest, 2, Placeholder.WheelPlaceholder.WheelCrossSectionPlaceholder);
        }

        [Test]
        public void TestWheelName()
        {
            testValue = extractor.GetArticleName();
            Check("KUMHO Z13 185/70 R19");
        }

        [Test]
        public void TestWheelDescription()
        {
            testValue = extractor.GetDescription();
            Check(wheelToTest.CrossSection);
        }

        [Test]
        public void TestWinterCategory()
        {
            testValue = extractor.GetCategory();
            Check(CSVConstants.WinterWheelCategoryName);
        }
        
        [Test]
        public void TestSummerCategory()
        {
            wheelToTest.IsMudSnow = false;
            wheelToTest.IsWinter = false;
            extractor = new PrestoshopWheelPropertyExtractor(wheelToTest, 2, "");
            testValue = extractor.GetCategory();
            Check(CSVConstants.SummerWheelCategoryName);
        }

        [Test]
        public void TestAllYearWheelCategory()
        {
            wheelToTest.IsMudSnow = true;
            wheelToTest.IsWinter = false;
            extractor = new PrestoshopWheelPropertyExtractor(wheelToTest, 2, "");
            testValue = extractor.GetCategory();
            Check(CSVConstants.AllYearWheelCategoryName);
        }

        [Test]
        public void TestFeaturesContainsWheelSize()
        {
            testValue = extractor.GetArticleFeatures();
            CheckContains("Reifenbreite:185:1:");
        }

        [Test]
        public void TestFeaturesContainsWheelType()
        {
            testValue = extractor.GetArticleFeatures();
            CheckContains("Typ:Winterreifen:7:");
        }

        [Test]
        public void TestFeaturesContainsSpeedIndex()
        {
            testValue = extractor.GetArticleFeatures();
            CheckContains("Geschwindigkeitsindex:N:4:");
        }

        [Test]
        public void TestFeaturesContainsCrossSection()
        {
            testValue = extractor.GetArticleFeatures();
            CheckContains(",Felgendurchmesser:R19:3:");
        }

        [Test]
        public void TestWheelHasNoDotFeature()
        {
            testValue = extractor.GetArticleFeatures();
            Assert.IsFalse(testValue.Contains("DOT"));
        }

        [Test]
        public void TestWheelHasDotFeature()
        {
            wheelToTest.DotNumber = 2010;
            extractor = new PrestoshopWheelPropertyExtractor(wheelToTest, 2, "");
            testValue = extractor.GetArticleFeatures();
            Assert.IsTrue(testValue.Contains("DOT:2010:8:"));
        }

        private void CheckContains(string expectedValue)
        {
            Assert.IsTrue(testValue.Contains(expectedValue));
        }

        private void Check(string expectedValue)
        {
            Assert.AreEqual(expectedValue, testValue);
        }

        private IWheel GetWheelToTest()
        {
            return new Wheel()
                {
                    Description = "testdescription",
                    WheelId = "Z13",
                    WheelWidth = 185,
                    WheelHeight = 70,
                    CrossSection = "R19",
                    WeightIndex = 70,
                    AcousticLevel = "90S",
                    SpeedIndex = 'N',
                    Manufactorer = "KUMHO",
                    IsWinter = true
                };
        }
    }
}
