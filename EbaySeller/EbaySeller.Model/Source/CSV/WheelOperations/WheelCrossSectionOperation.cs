using EbaySeller.Common.DataInterface;
using EbaySeller.Model.Source.CSV.WheelOperations.Interfaces;

namespace EbaySeller.Model.Source.CSV.WheelOperations
{
    public class WheelCrossSectionOperation: IWheelOperation
    {
        public IWheel SetValueOnWheel(IWheel wheel, string pattern)
        {
            wheel.CrossSection = pattern;
            return wheel;
        }

        public string GetRegexPattern()
        {
            return @"R\s*\d{2,3}";
        }
    }
}
