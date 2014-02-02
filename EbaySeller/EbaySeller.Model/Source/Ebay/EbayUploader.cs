using System.Collections.Generic;
using System.Configuration;
using EbaySeller.Common.DataInterface;
using EbaySeller.Model.Source.CSV.Writer;
using EbaySeller.Model.Source.Ebay.ArticleBuilders;
using EbaySeller.Model.Source.Ebay.Constants;
using EbaySeller.Model.Source.Ebay.Facade;
using EbaySeller.Model.Source.Ebay.Interfaces;
using eBay.Service.Core.Soap;

namespace EbaySeller.Model.Source.Ebay
{
    public class EbayUploader:IEbayUploader
    {
        private static readonly BuyerPaymentMethodCodeType[] PaymentMethods = new[]
            {
                BuyerPaymentMethodCodeType.MoneyXferAccepted, 
                BuyerPaymentMethodCodeType.PayPal
            };

        private readonly IEbayFacade facade;
        private double currentAmount;
        private readonly string currentMail;
        private readonly EbayItemSpecificsBuilder itemSpecificBuilder;

        public EbayUploader()
        {
            currentMail = ConfigurationManager.AppSettings["Paypal.Mail"];
            itemSpecificBuilder = new EbayItemSpecificsBuilder();
            facade = new EbayFacade();
        }

        public IArticle RefreshOrCreateEbayArticle(IArticle article, EbayArticleCSVWriter cswWriter, double amount, string template)
        {
            IArticle newArticle = null;
            currentAmount = amount;
           
            var availabilityMapToIterate = new Dictionary<int, int>();
            availabilityMapToIterate[2] = 6;
            availabilityMapToIterate[1] = 6;
            availabilityMapToIterate[4] = 8;
            
            var availabilityMap = new Dictionary<int, int>(availabilityMapToIterate);
            var rest = article.Availability - 6;
            foreach (var item in availabilityMapToIterate)
            {
                rest = rest - item.Value;
                if (rest < 0)
                {
                    availabilityMap[item.Key] = 0;
                }
            }

            foreach (var availabilityPair in availabilityMap)
            {
                if (article.EbayIds.ContainsKey(availabilityPair.Key))
                {
                    newArticle = ReviseEbayArticle(article, availabilityPair);
                    continue;
                }
                if (availabilityPair.Value == 0)
                {
                    continue;
                }
                newArticle = LoadUpNewSingleArticle(article, template, availabilityPair);
            }

            cswWriter.WriteToCSVFile(newArticle);
            return newArticle;
        }

        private IArticle ReviseEbayArticle(IArticle article, KeyValuePair<int, int> availabilityPair)
        {
            var ebayType = new ItemType();

            ebayType.ItemID = article.EbayIds[availabilityPair.Key];
            ebayType.OutOfStockControl = true;
            ebayType.Quantity = GetQuantity(availabilityPair);
            ebayType.StartPrice = GetCalculatedPrice(article, availabilityPair.Key);
            ebayType.VATDetails = GetVatDetails();
            ebayType.Storefront = GetStorefrontType();
 
            return article;
        }

        private static int GetQuantity(KeyValuePair<int, int> availabilityPair)
        {
            return availabilityPair.Value/availabilityPair.Key;
        }

        private IArticle LoadUpNewSingleArticle(IArticle article, string template, KeyValuePair<int, int> availabilityPair)
        {
            var ebayType = new ItemType();

            var wheel = article as IWheel;
            if (wheel != null)
            {
                ebayType.Title = GetTitleFromArticle(wheel, availabilityPair.Key);
                ebayType.ItemSpecifics = itemSpecificBuilder.GetItemSpecifics(wheel);
            }

            ebayType.Description = GetDescriptionFromArticle(wheel, template, availabilityPair.Key);
            ebayType.StartPrice = GetCalculatedPrice(article, availabilityPair.Key);
            ebayType.Quantity = GetQuantity(availabilityPair);

            ebayType.ListingType = ListingTypeCodeType.StoresFixedPrice;
            ebayType.ListingDuration = "GTC";
            ebayType.Currency = CurrencyCodeType.EUR;
            ebayType.Location = "Baden-Baden";
            ebayType.Country = CountryCodeType.DE;
            var category = new CategoryType();
            category.CategoryID = "9891";
            ebayType.PrimaryCategory = category;
            ebayType.ConditionID = 1000;
            ebayType.PaymentMethods = new BuyerPaymentMethodCodeTypeCollection(PaymentMethods);
            ebayType.PayPalEmailAddress = currentMail;
            ebayType.DispatchTimeMax = 1;

            ebayType.ShippingDetails = GetShippingDetails(availabilityPair.Key);
            
            ebayType.MotorsGermanySearchable = true;
            ebayType.VATDetails = GetVatDetails();

            ebayType.Storefront = GetStorefrontType();

            ebayType.ReturnPolicy = GetPolicy();
            ebayType.PictureDetails = GetPictureDetails(article);

            article.EbayIds[availabilityPair.Key] = facade.AddFixedPriceItem(ebayType);
            return article;
        }

        private StorefrontType GetStorefrontType()
        {
            return new StorefrontType {StoreCategoryID = 3492568016};
        }

        private VATDetailsType GetVatDetails()
        {
            return new VATDetailsType {VATPercent = 0.19f};
        }

        private PictureDetailsType GetPictureDetails(IArticle article)
        {
            var pictureDetails = new PictureDetailsType();
            pictureDetails.PictureURL = new StringCollection();
            pictureDetails.PictureURL.Add(article.ImageLink);
            pictureDetails.PhotoDisplay = PhotoDisplayCodeType.None;
            return pictureDetails;
        }

        private AmountType GetCalculatedPrice(IArticle article, int articleCount)
        {
            double price = article.Price * articleCount;
            price += currentAmount*articleCount + 0.35;
            price /= (EbayArticleConstants.CalculatedConstant);

            return new AmountType {currencyID = CurrencyCodeType.EUR, Value = price};
        }

        private ShippingDetailsType GetShippingDetails(int countOfArticles)
        {
            ShippingDetailsType sd = new ShippingDetailsType();
            sd.ShippingType = ShippingTypeCodeType.Flat;

            ShippingServiceOptionsType st1 = new ShippingServiceOptionsType();
            st1.ShippingService = ShippingServiceCodeType.DE_Paket.ToString();
            st1.ShippingServiceAdditionalCost = new AmountType { Value = 0, currencyID = CurrencyCodeType.EUR };
            st1.ShippingServicePriority = 1;
            st1.ShippingServiceCost = new AmountType { Value = 5.95, currencyID = CurrencyCodeType.EUR };

            if (countOfArticles > 1)
            {
                st1.ShippingServiceCost.Value = 0;
            }

            sd.ShippingServiceOptions = new ShippingServiceOptionsTypeCollection(new[] { st1});
            return sd;
        }

        public static ReturnPolicyType GetPolicy()
        {
            ReturnPolicyType policy = new ReturnPolicyType();
            policy.Description = EbayArticleConstants.EbayReturnFurtherInformation;
            policy.ReturnsWithinOption = "Days_14";
            policy.ReturnsAcceptedOption = "ReturnsAccepted";
            policy.ShippingCostPaidByOption = "Buyer";
            return policy;
        }

        private string GetDescriptionFromArticle(IWheel article, string template, int countOfArticles)
        {
            return string.Format(template, 
                article.ImageLink, 
                GetTitleFromArticle(article, countOfArticles), 
                article.Manufactorer, 
                article.WheelWidth, 
                article.WheelHeight, 
                article.CrossSection,
                article.WeightIndex, 
                article.SpeedIndex,
                article.TyreLabelLink);
        }

        private static string GetTitleFromArticle(IArticle article, int countOfArticles)
        {
            var wheel = article as IWheel;
            if (wheel != null)
            {
                if (wheel.IsWinter)
                {
                    return string.Format(EbayArticleConstants.EbayTitleWinterWheelTemplate, wheel.Description);
                }
                if (wheel.IsMudSnow)
                {
                    return string.Format(EbayArticleConstants.EbayTitleAllWeatherWheelTemplate,countOfArticles, wheel.Description);
                }
                return string.Format(EbayArticleConstants.EbayTitleSummerWheelTemplate, countOfArticles, wheel.Description);
            }
            
            return article.Description;
        }

        public void RemoveAllEbayArticles(IArticle articleToDelete)
        {
            
            DeleteArticle(articleToDelete, 1);
            DeleteArticle(articleToDelete, 2);
            DeleteArticle(articleToDelete, 4);
        }

        private void DeleteArticle(IArticle articleToDelete, int index)
        {
            if (articleToDelete.EbayIds.ContainsKey(index))
            {
                facade.DeleteEbayItem(articleToDelete.EbayIds[index]);
            }
        }
    }
}
