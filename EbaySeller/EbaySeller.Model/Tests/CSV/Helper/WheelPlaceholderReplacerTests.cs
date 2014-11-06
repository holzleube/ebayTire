using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using EbaySeller.Common.DataInterface;
using EbaySeller.Model.Source.CSV.Extractors.Helper;
using EbaySeller.Model.Source.Data;
using NUnit.Framework;

namespace EbaySeller.Model.Tests.CSV.Helper
{
    [TestFixture]
    public class WheelPlaceholderReplacerTests
    {
        private IPlaceholderReplacer replacer;
        private IWheel testWheel;
        private string testValue;

        [SetUp]
        public void InitReplacer()
        {
            testValue = "";
            testWheel = GetTestWheel();
            this.replacer = new WheelPlaceholderReplacer(testWheel);
        }

        [Test]
        public void TestWheelWidth()
        {
            TestReplacement(Placeholder.WheelPlaceholder.WheelWidthPlaceholder, testWheel.WheelWidth.ToString(CultureInfo.InvariantCulture));
        }
        
        [Test]
        public void TestWheelHeight()
        {
            TestReplacement(Placeholder.WheelPlaceholder.WheelHeightPlaceholder, testWheel.WheelHeight.ToString(CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestWheelName()
        {
            TestReplacement(Placeholder.WheelPlaceholder.WheelNamePlaceholder, testWheel.WheelId);
        }

        [Test]
        public void TestWheelCrossSection()
        {
            TestReplacement(Placeholder.WheelPlaceholder.WheelCrossSectionPlaceholder, testWheel.CrossSection);
        }
        
        [Test]
        public void TestWheelLoadIndex()
        {
            TestReplacement(Placeholder.WheelPlaceholder.WheelLoadIndex, testWheel.WeightIndex.ToString(CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestWheelSpeedIndex()
        {
            TestReplacement(Placeholder.WheelPlaceholder.WheelSpeedIndex, testWheel.SpeedIndex.ToString(CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestWheelEuImageLink()
        {
            TestReplacement(Placeholder.WheelPlaceholder.WheelEuImageLink, testWheel.TyreLabelLink);
        }

        private void TestReplacement(string placeholder, string expectedValue)
        {
            testValue = replacer.Replace(placeholder);
            Check(expectedValue);
        }

        private void Check(string expectedValue)
        {
            Assert.AreEqual(expectedValue, testValue);
        }

        private IWheel GetTestWheel()
        {
            return new Wheel()
                {
                    WheelWidth = 80,
                    WheelHeight = 175,
                    CrossSection = "R19", 
                    WeightIndex = 90,
                    SpeedIndex = 'R',
                    TyreLabelLink = "link",
                    WheelId = "nskk234"
                };
        }
    }
}
