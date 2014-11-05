using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EbaySeller.Common.DataInterface;
using EbaySeller.Model.Source.CSV.Writer;
using EbaySeller.Model.Source.Data;
using NUnit.Framework;

namespace EbaySeller.Model.Tests.CSV
{
    [TestFixture]
    public class PrestoshopCSsvConverterTestscs
    {
        private ICSVWriter prestoShopWriter;
        private const string TestLine = "S2554017ZYOSDRIVE;1;YOKOHAMA S-DRI  255/40 R17;Winterreifen;100,00;53;20;0;;;;;;;;YOKOHAMA;;;;;;;;70;1;;;;;kurze Beschreibung;Beschreibung;Tags;;;;;;;1;;;1;http://media2.tyre24.de/images/tyre/3052-NTI2NzY=-w300-h300-br1.jpg;0;;0;new;0;0;0;0;0;0;0;0";

        [SetUp]
        public void InitTestClass()
        {
            prestoShopWriter = new PrestoShopCSVWriter("",2);
        }

        [Test]
        public void TestRightStringFromArticle()
        {
            var articleToTest = GetTestArticle();
            var resultSTring= prestoShopWriter.GetTextLineFromArticle(articleToTest);
            Assert.AreEqual(TestLine, resultSTring);
        }

        private IArticle GetTestArticle()
        {
            return new Wheel
                {
                    Id = 1,
                    AnonymPrice = 1,
                    Description = "YOKOHAMA S-DRI  255/40 R17",
                    WheelWidth = 40,
                    WheelHeight = 255,
                    CrossSection = "R17",
                    Availability = 70,
                    Price = 98,
                    Manufactorer = "YOKOHAMA",
                    ArticleId = "S2554017ZYOSDRIVE",
                    ImageLink = "http://media2.tyre24.de/images/tyre/3052-NTI2NzY=-w300-h300-br1.jpg",
                    IsWinter = true,
                    
                };
        }
    }

}
