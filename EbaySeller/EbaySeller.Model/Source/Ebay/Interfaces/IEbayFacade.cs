using eBay.Service.Core.Soap;

namespace EbaySeller.Model.Source.Ebay.Interfaces
{
    public interface IEbayFacade
    {
        string AddFixedPriceItem(ItemType ebayItemToCreate);

        void DeleteEbayItem(string ebayId);

        void ReviseEbayArticle(ItemType itemToRevise);
    }
}
