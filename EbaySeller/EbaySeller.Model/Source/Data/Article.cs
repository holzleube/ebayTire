using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EbaySeller.Model.Source.Data.Interfaces;

namespace EbaySeller.Model.Source.Data
{
    public class Article : IArticle
    {
        public int Id { get; set; }
        public string ArticleId { get; set; }
        public string Description { get; set; }
        public string Description2 { get; set; }
        public double Price { get; set; }
        public double Price4 { get; set; }
        public double AvgPrice { get; set; }
        public double AnonymPrice { get; set; }
        public double RvoPrice { get; set; }
        public int Availability { get; set; }
        public string ImageLink { get; set; }
        public string ImageTnLink { get; set; }
        public string InfoLink { get; set; }
        public string Manufactorer { get; set; }
        public string ManufactorerNumber { get; set; }
        public string DirectLink { get; set; }
        public string TyreLabelLink { get; set; }
        public string EbayId { get; set; }
        public bool IsToDelete { get; set; }
    }
}
