using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EbaySeller.Model.Source.CSV.Extractors.Helper
{
    public class Placeholder
    {
        internal class ArticlePlaceholder
        {
            public const string NamePlaceholder = "$name$";
            public const string DescriptionPlaceholder =  "$description$";
            public const string ManufactorPlaceholder = "$Manufactorer$";
        }

        internal class WheelPlaceholder
        {
            public const string WheelNamePlaceholder = "$wheelId$";
            public const string WheelWidthPlaceholder = "$wheelWidth$";
            public const string WheelHeightPlaceholder = "$wheelHeigth$";
            public const string WheelCrossSectionPlaceholder = "$wheelCrossSection$";
            public const string WheelLoadIndex = "$loadIndex$";
            public const string WheelSpeedIndex = "$speedIndex$";
            public const string WheelEuImageLink = "$euImage$";
            public const string WheelNameTemplate = WheelNamePlaceholder + " " + WheelWidthPlaceholder + "/" + WheelHeightPlaceholder + " " + WheelCrossSectionPlaceholder;
        }
    }
}
