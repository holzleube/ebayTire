using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EbaySeller.Common.DataInterface;
using EbaySeller.Model.Source.CSV.Constants;
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
            var result = placeholderReplacer.Replace(CSVConstants.WheelSizeTemplate);
            placeholderReplacer.Replace(descriptionTemplate);
            if (wheel.DotNumber > 1990)
            {
                result += ", " + string.Format(CSVConstants.WheelDotTemplate, wheel.DotNumber);
            }
            return result;
        }

        public override string GetCategory()
        {
            return placeholderReplacer.Replace(Placeholder.WheelPlaceholder.WheelType);
        }
    }
}
