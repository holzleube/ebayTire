using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using EbaySeller.Common.DataInterface;

namespace EbaySeller.Model.Source.CSV.Extractors.Helper
{
    public class WheelPlaceholderReplacer:PlaceholderReplacer
    {
        public WheelPlaceholderReplacer(IWheel article) : base(article)
        {
            replaceMap.Add(Placeholder.WheelPlaceholder.WheelHeightPlaceholder, article.WheelHeight.ToString(CultureInfo.InvariantCulture));
            replaceMap.Add(Placeholder.WheelPlaceholder.WheelWidthPlaceholder, article.WheelWidth.ToString(CultureInfo.InvariantCulture));
            replaceMap.Add(Placeholder.WheelPlaceholder.WheelNamePlaceholder, article.WheelId);
            replaceMap.Add(Placeholder.WheelPlaceholder.WheelEuImageLink, article.TyreLabelLink);
            replaceMap.Add(Placeholder.WheelPlaceholder.WheelLoadIndex, article.WeightIndex.ToString(CultureInfo.InvariantCulture));
            replaceMap.Add(Placeholder.WheelPlaceholder.WheelSpeedIndex, article.SpeedIndex.ToString(CultureInfo.InvariantCulture));
            replaceMap.Add(Placeholder.WheelPlaceholder.WheelCrossSectionPlaceholder, article.CrossSection);
        }
    }
}
