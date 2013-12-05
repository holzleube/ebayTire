using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EbaySeller.Model.Source.Data.Interfaces;

namespace EbaySeller.Model.Source.Data
{
    public class Wheel:Article, IWheel
    {
        public Wheel()
        {
            
        }
        public Wheel(IArticle article)
        {
            Id = article.Id;
            ArticleId = article.ArticleId;
            Description = article.Description;
            Description2 = article.Description2;
            Price = article.Price;
            Price4 = article.Price4;
            AvgPrice = article.AvgPrice;
            AnonymPrice = article.AnonymPrice;
            RvoPrice = article.RvoPrice;
            Availability = article.Availability;
            ImageLink = article.ImageLink;
            InfoLink = article.InfoLink;
            Manufactorer = article.Manufactorer;
            ManufactorerNumber = article.ManufactorerNumber;
            DirectLink = article.DirectLink;
            TyreLabelLink = article.TyreLabelLink;
        }

        //wheel specific values
        public string ManufactorerShortName { get; set; }
        public string WheelId { get; set; }
        public int WheelWidth { get; set; }
        public int WheelHeight { get; set; }
        public string CrossSection { get; set; }
        public int WeightIndex { get; set; }
        public char SpeedIndex { get; set; }
        public string FuelNeed { get; set; }
        public string BreakingDistance { get; set; }
        public string AcousticLevel { get; set; }
        public bool IsWinter { get; set; }
        public bool IsMudSnow { get; set; }
        public int DotNumber { get; set; }
    }
}
