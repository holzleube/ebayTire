using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EbaySeller.Model.Source.Images;
using NUnit.Framework;

namespace EbaySeller.Model.Tests.Images
{
    [TestFixture]
    public class ImageLoaderTests
    {
        [Test]
        public void TestLoadingSampleImage()
        {
            var loader = new ImageLoader(@"C:\Users\Dane\Dropbox\test.jpg");
            loader.SaveImageToFile("http://media1.tyre24.de/tyrelabel/7/7/3/71/1-h300-w300.jpg", "name.jpg");
        }
    }
}
