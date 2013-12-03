using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EbaySeller.Model.Source.CSV.WheelOperations.Interfaces;
using EbaySeller.Model.Source.Data;
using EbaySeller.Model.Source.Data.Interfaces;

namespace EbaySeller.Model.Source.CSV.WheelOperations
{
    public static class WheelOperationHandler
    {
        private static IEnumerable<IWheelOperation> wheelsOperations = new List<IWheelOperation>()
            {
                new WheelManufactorerShortNameOperation(),
                new WheelArticleNameOperation(),
                new WheelCrossSectionOperation(),
                new WheelWeightIndexOperation(),
                new WheelSpeedIndexOperation()
            };
        private static IWheelOperation heightWidthOperation = new WheelHeightWidthOperation();

        public static IArticle GetWheelForDescription(string descriptionString)
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
            foreach(IWheelOperation operation in wheelsOperations)
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

        private static bool IsNoCarWheel(string descriptionString)
        {
            var testPattern = AWheelOperations.GetPattern(descriptionString, @"\d{3}/\d{2}");
            return testPattern.Equals(string.Empty);
        }
    }
}
