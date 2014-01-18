namespace EbaySeller.Common.DataInterface
{
    public interface IWheel:IArticle
    {
        string ManufactorerShortName { get; set; }

        string WheelId { get; set; }

        int WheelWidth { get; set; }

        int WheelHeight { get; set; }

        string CrossSection { get; set; }

        int WeightIndex { get; set; }

        char SpeedIndex { get; set; }

        string FuelNeed { get; set; }

        string BreakingDistance { get; set; }

        string AcousticLevel { get; set; }

        bool IsWinter { get; set; }

        bool IsMudSnow { get; set; }

        int DotNumber { get; set; }
    }
}
