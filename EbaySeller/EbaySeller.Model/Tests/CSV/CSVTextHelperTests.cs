using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EbaySeller.Model.Source.CSV;
using NUnit.Framework;

namespace EbaySeller.Model.Tests.CSV
{
    [TestFixture]
    public class CSVTextHelperTests
    {
        private const string TEST_LINE =
            "184098|B20050XKIKT207|\"KINGSTIR KT207  200    -50 2 PR    \"||10.01||10.01||19.6|2||http://media1.tyre24.de/images/tyre/nopic_nobr-NTI2NzY=-w300-h300-br1.jpg|http://media1.tyre24.de/images/tyre/nopic_nobr-NTI2NzY=-w112-h150-br1.jpg|http://www.tyre24.de/item/profil/id/184098/allow/NTI2NzY=|KINGSTIRE|http://www.tyre24.de/reifen/profi?search=ID184098|http://media3.tyre24.de/tyrelabel/0/0/0-h300-w300.jpg";

        //private const string SECOND_TEST_LINE = "255108|O2755517HFUYUKON|\"FULDA    YUKON  275/55 R17 109H"|"WINTERREIFEN M+S\"|137.89|137.89|137.89||213.7|12||http://media2.tyre24.de/images/tyre/154-NTI2NzY=-w300-h300-br1.jpg|http://media2.tyre24.de/images/tyre/154-NTI2NzY=-w112-h150-br1.jpg|http://www.tyre24.de/item/profil/id/255108/allow/NTI2NzY=|FULDA|http://www.tyre24.de/reifen/profi?search=ID255108|http://media3.tyre24.de/tyrelabel/0/0/0/0/0-h300-w300.jpg";

        private CSVTextHelper textReader;

        [SetUp]
        public void InitializeCSVReader()
        {
            this.textReader = new CSVTextHelper();
        }

        //[Test]
        //public void SecondTestLine()
        //{
        //   var result= CSVTextHelper.GetArticleFromString(SECOND_TEST_LINE, 34);
        //    Assert.IsNotNull(result);
        //}
        [Test]
        public void TestReadArticlePositiveCase()
        {
            var articleToTest = CSVTextHelper.GetArticleFromString(TEST_LINE, 1);
            Assert.AreEqual(1, articleToTest.Id);
            Assert.AreEqual("B20050XKIKT207", articleToTest.ArticleId);
            Assert.AreEqual("KINGSTIR KT207  200    -50 2 PR    ", articleToTest.Description);
            Assert.AreEqual("", articleToTest.Description2);
            Assert.AreEqual(10.01, articleToTest.Price);
            Assert.AreEqual(0, articleToTest.Price4);
            Assert.AreEqual(10.01, articleToTest.AvgPrice);
            Assert.AreEqual(0, articleToTest.AnonymPrice);
            Assert.AreEqual(19.6, articleToTest.RvoPrice);
            Assert.AreEqual(2, articleToTest.Availability);
            Assert.AreEqual("", articleToTest.ManufactorerNumber);
            Assert.AreEqual("http://media1.tyre24.de/images/tyre/nopic_nobr-NTI2NzY=-w300-h300-br1.jpg", articleToTest.ImageLink);
            Assert.AreEqual("http://media1.tyre24.de/images/tyre/nopic_nobr-NTI2NzY=-w112-h150-br1.jpg", articleToTest.ImageTnLink);
            Assert.AreEqual("http://www.tyre24.de/item/profil/id/184098/allow/NTI2NzY=", articleToTest.InfoLink);
            Assert.AreEqual("KINGSTIRE", articleToTest.Manufactorer);
            Assert.AreEqual("http://www.tyre24.de/reifen/profi?search=ID184098", articleToTest.DirectLink);
            Assert.AreEqual("http://media3.tyre24.de/tyrelabel/0/0/0-h300-w300.jpg", articleToTest.TyreLabelLink);
        }
    }
}
