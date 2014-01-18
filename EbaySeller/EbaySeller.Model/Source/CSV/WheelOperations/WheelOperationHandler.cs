using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EbaySeller.Common.DataInterface;
using EbaySeller.Model.Source.CSV.WheelOperations.Interfaces;
using EbaySeller.Model.Source.Data;

namespace EbaySeller.Model.Source.CSV.WheelOperations
{
    public static class WheelOperationHandler
    {
        private static readonly IEnumerable<IWheelOperation> wheelsOperations = new List<IWheelOperation>()
            {
                new WheelManufactorerShortNameOperation(),
                new WheelArticleNameOperation(),
                new WheelCrossSectionOperation(),
                new WheelWeightIndexOperation(),
                new WheelSpeedIndexOperation()
            };
        
        private static readonly IEnumerable<IWheelOperation> extraWheelsOperations = new List<IWheelOperation>()
            {
                new WheelDotOperation(),
                new WheelIsWinterOperation(),
                new WheelMudAndSnowOperation()
            };

        private static readonly IWheelOperation heightWidthOperation = new WheelHeightWidthOperation();

        public static IArticle GetWheelForDescription(string descriptionString, string description2String)
        {
            IWheel wheel = new Wheel();
            var heightWidthPattern = AWheelOperations.GetPattern(descriptionString,
                                                                 heightWidthOperation.GetRegexPattern());
            if (heightWidthPattern.Equals(string.Empty))
            {
                return wheel;
            }
            wheel = heightWidthOperation.SetValueOnWheel(wheel, heightWidthPattern);
            descriptionString = AWheelOperations.CutFromString(descriptionString, heightWidthPattern);
            wheel = GetWheelWithGivenWheelOperations(descriptionString, wheel, wheelsOperations);
            wheel = GetWheelWithGivenWheelOperations(description2String, wheel, extraWheelsOperations);
            return wheel;
        }

        private static IWheel GetWheelWithGivenWheelOperations(string descriptionString, IWheel wheel, IEnumerable<IWheelOperation> wheelOperations)
        {
            foreach (IWheelOperation operation in wheelOperations)
            {
                var pattern = AWheelOperations.GetPattern(descriptionString, operation.GetRegexPattern());
                if (pattern.Equals(string.Empty))
                {
                    continue;
                }
                wheel = operation.SetValueOnWheel(wheel, pattern);
                descriptionString = AWheelOperations.CutFromString(descriptionString, pattern);
            }
            return wheel;
        }
    }
}
