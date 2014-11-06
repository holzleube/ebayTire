using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EbaySeller.Common.DataInterface;
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
        public void TestWheelFeature()
        {
            testValue = extractor.GetArticleFeatures();
            Check("Breite:185:1:");
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
                    WheelId = "KUMHO Z13",
                    WheelWidth = 185,
                    WheelHeight = 70,
                    CrossSection = "R19",
                    WeightIndex = 70,
                    AcousticLevel = "90S",
                    SpeedIndex = 'N'
                };
        }
    }
}
