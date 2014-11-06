using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EbaySeller.Common.DataInterface;
using EbaySeller.Model.Source.CSV.Extractors.Helper;

namespace EbaySeller.Model.Source.CSV.Extractors
{
    public class PrestoshopWheelPropertyExtractor:PrestoshopPropertyExtractor
    {
        private IWheel wheel;
        private string descriptionTemplate;

        public PrestoshopWheelPropertyExtractor(IWheel articleToUse, double marge, string descriptionTemplate) : base(articleToUse, marge, new WheelPlaceholderReplacer(articleToUse))
        {
            this.wheel = articleToUse;
            this.descriptionTemplate = descriptionTemplate;
        }

        public override string GetArticleName()
        {
            return placeholderReplacer.Replace(Placeholder.WheelPlaceholder.WheelNameTemplate);
        }

        public override string GetDescription()
        {
            return placeholderReplacer.Replace(descriptionTemplate);
        }

        public override string GetArticleFeatures()
        {
            return string.Format("Breite:{0}:1:", wheel.WheelWidth);
        }
    }
}
