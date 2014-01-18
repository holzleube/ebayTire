namespace EbaySeller.Common.DataInterface
{
    public interface IArticle
    {
        int Id { get; set; }

        string ArticleId { get; set; }

        string Description { get; set; }

        string Description2 { get; set; }

        double Price { get; set; }

        double Price4 { get; set; }

        double AvgPrice { get; set; }

        double AnonymPrice { get; set; }

        double RvoPrice { get; set; }

        int Availability { get; set; }

        string ImageLink { get; set; }

        string ImageTnLink { get; set; }

        string InfoLink { get; set; }

        string Manufactorer { get; set; }

        string ManufactorerNumber { get; set; }

        string DirectLink { get; set; }

        string TyreLabelLink { get; set; }

        string EbayId { get; set; }

        bool IsToDelete { get; set; }

    }
}
