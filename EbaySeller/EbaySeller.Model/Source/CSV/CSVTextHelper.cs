using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EbaySeller.Model.Source.Data;
using EbaySeller.Model.Source.Data.Interfaces;

namespace EbaySeller.Model.Source.CSV
{
    public class CSVTextHelper
    {
        private static List<string> tyreManufactorer = new List<string>()
            {
                "MICHELIN"
            }; 
        public static IArticle GetArticleFromString(string textLine, int articleId)
        {
            var values = textLine.Split('|');
            var article = new Article();
            var description = GetString(values[2]);
            var manufactorer = GetString(values[14]);
            var tyreLabel = GetString(values[16]);
            if (IsWheel(description, manufactorer, tyreLabel))
            {
                article = GetWheel(description);
            }
            article.Id = articleId;
            article.ArticleId = GetString(values[1]);
            article.Description = GetString(values[2]);
            article.Description2 = GetString(values[3]);
            article.Price = GetDouble(values[4]);
            article.Price4 = GetDouble(values[5]);
            article.AvgPrice = GetDouble(values[6]);
            article.AnonymPrice = GetDouble(values[7]);
            article.RvoPrice = GetDouble(values[8]);
            article.Availability = Convert.ToInt32(values[9]);
            article.ManufactorerNumber = GetString(values[10]);
            article.ImageLink = values[11];
            article.ImageTnLink = values[12];
            article.InfoLink = values[13];
            article.Manufactorer = values[14];
            article.DirectLink = values[15];
            article.TyreLabelLink = values[16];
            return article;
        }

        private static Article GetWheel(string descriptionLine)
        {
            return new Wheel();
        }

        private static bool IsWheel(string textLine, string manufactorer, string tyrelabel)
        {
            if (tyrelabel.Contains(@"0/0/0/0/0-h300-w300.jpg"))
            {
                return false;
            }
            return true;
            //if (tyreManufactorer.Contains(manufactorer))
            //{
            //    return true;
            //}
            ////var match = Regex.Match(textLine, @"[A-Za-z]+\s*[A-Za-z]+ \s*\d{3}/\d{3}");
            //var match = Regex.Match(textLine, @"\d{3}/\d{3}");
            //return match.Success;
        }

        private static string GetString(string value)
        {
            var convertedValue = value.Replace("\"", "");
            return convertedValue;
        }

        private static double GetDouble(string value)
        {
            return value.Equals(String.Empty) ? 0.0 : double.Parse(value, CultureInfo.InvariantCulture);
        }
    }
}
