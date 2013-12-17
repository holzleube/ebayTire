using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EbaySeller.Model.Source.Data.Interfaces;

namespace EbaySeller.Model.Source.CSV.Writer
{
    public class EbayArticleCSVWriter:ICSVWriter
    {
        private string fileName;

        public EbayArticleCSVWriter(string filename)
        {
            this.fileName = filename;
        }
        public void WriteToCSVFile(IArticle articleToWrite)
        {
            string csvTextLine = GetTextLineFromArticle(articleToWrite);
            using (var file = new StreamWriter(@fileName))
            {
                file.WriteLine(csvTextLine);
            }
        }

        private string GetTextLineFromArticle(IArticle articleToWrite)
        {
            return "line"+articleToWrite.Description;
        }
    }
}
