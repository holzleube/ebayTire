using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace EbaySeller.Model.Source.Images
{
    public class ImageLoader
    {
        private string imagePath;

        public ImageLoader(string imagePath)
        {
            this.imagePath = imagePath;
        }

        public void SaveImageToFile(string imageUrl, string imageName)
        {
            string fileName = System.IO.Path.Combine(imagePath, imageName);
            var webClient = new WebClient();
            webClient.DownloadFile(imageUrl, fileName);
        }
    }
}
