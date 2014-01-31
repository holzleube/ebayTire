using System;
using System.Collections.Generic;
using System.Configuration;
using EbaySeller.Common.DataInterface;
using EbaySeller.Model.Source.CSV.Writer;
using EbaySeller.Model.Source.Ebay.ArticleBuilders;
using EbaySeller.Model.Source.Ebay.Constants;
using EbaySeller.Model.Source.Ebay.Facade;
using EbaySeller.Model.Source.Ebay.Interfaces;
using EbaySeller.Model.Source.Ebay.Template;
using eBay.Service.Call;
using eBay.Service.Core.Sdk;
using eBay.Service.Core.Soap;
using eBay.Service.Util;
using log4net;
using log4net.Repository.Hierarchy;

namespace EbaySeller.Model.Source.Ebay
{
    public class EbayUploader:IEbayUploader
    {
        private ILog logger = LogManager.GetLogger(typeof(EbayUploader));
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

            var calculatedCount = article.Availability - 6;
            if (calculatedCount < 1)
            {
                var countOf2Articles = calculatedCount + 6;
            }
            calculatedCount = calculatedCount - 6;
            if (calculatedCount < 1)
            {
                var countOf1Articles = calculatedCount + 6;
            }
            calculatedCount = calculatedCount - 8;
            if (calculatedCount < 1)
            {
                var countOf4Articles = calculatedCount + 8;
            }

            
            if (string.IsNullOrEmpty(article.EbayId))
            {
                if (article.Availability < EbayArticleConstants.MinimumCountOfArticles)
                {
                    return article;
                }
                if (article.Availability > 20)
                {
                    newArticle = LoadUpNewSingleArticle(article, template);
                    newArticle = LoadUpNewSingleArticle(article, template, 3);
                    newArticle = LoadUpNewSingleArticle(article, template, 2);
                }
                newArticle = LoadUpNewSingleArticle(article, template);
            }
            else
            {
                newArticle = ReviseEbayArticle(article);
            }

            cswWriter.WriteToCSVFile(newArticle);
            return newArticle;
        }

        private IArticle LoadUpNewSingleArticle(IArticle article, string template, int i)
        {
            
        }


        private IArticle ReviseEbayArticle(IArticle article)
        {
            var ebayType = new ItemType();
            
            ebayType.ItemID = article.EbayId;
            ebayType.OutOfStockControl = true;
            ebayType.Quantity = GetQuantityOfArticle(article);
            ebayType.StartPrice = GetCalculatedPrice(article);
            ebayType.VATDetails = GetVatDetails();
            ebayType.Storefront = GetStorefrontType();
 
            return article;
        }

        private static int GetQuantityOfArticle(IArticle article)
        {
            if (article.Availability < 6)
            {
                return article.Availability;
            }
            return 6;
        }

        private IArticle LoadUpNewSingleArticle(IArticle article, string template)
        {
            var ebayType = new ItemType();

            var wheel = article as IWheel;
            if (wheel != null)
            {
                ebayType.Title = GetTitleFromArticle(wheel);
                ebayType.ItemSpecifics = itemSpecificBuilder.GetItemSpecifics(wheel);
            }

            ebayType.Description = GetDescriptionFromArticle(wheel, template);
            ebayType.StartPrice = GetCalculatedPrice(article);
            ebayType.Quantity = GetQuantityOfArticle(article);

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

            ebayType.ShippingDetails = GetShippingDetails();
            
            ebayType.MotorsGermanySearchable = true;
            ebayType.VATDetails = GetVatDetails();

            ebayType.Storefront = GetStorefrontType();

            ebayType.ReturnPolicy = GetPolicy();
            ebayType.PictureDetails = GetPictureDetails(article);

            article.EbayId = facade.AddFixedPriceItem(ebayType);
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

        private AmountType GetCalculatedPrice(IArticle article)
        {
            double price = article.Price;
            price += currentAmount + 0.35;
            price /= (EbayArticleConstants.CalculatedConstant);

            return new AmountType {currencyID = CurrencyCodeType.EUR, Value = price};
        }

        private ShippingDetailsType GetShippingDetails()
        {
            ShippingDetailsType sd = new ShippingDetailsType();
            sd.ShippingType = ShippingTypeCodeType.Flat;

            ShippingServiceOptionsType st1 = new ShippingServiceOptionsType();
            st1.ShippingService = ShippingServiceCodeType.DE_Paket.ToString();
            st1.ShippingServiceAdditionalCost = new AmountType() { Value = 0, currencyID = CurrencyCodeType.EUR };
            
            st1.ShippingServiceCost = new AmountType() { Value = 5.95, currencyID = CurrencyCodeType.EUR };
            st1.ShippingServicePriority = 1;
            
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

        private string GetDescriptionFromArticle(IWheel article, string template)
        {
            return string.Format(template, 
                article.ImageLink, 
                GetTitleFromArticle(article), 
                article.Manufactorer, 
                article.WheelWidth, 
                article.WheelHeight, 
                article.CrossSection,
                article.WeightIndex, 
                article.SpeedIndex,
                article.TyreLabelLink);
        }

        private string GetTitleFromArticle(IArticle article)
        {
            var wheel = article as IWheel;
            if (wheel != null)
            {
                if (wheel.IsWinter)
                {
                    return string.Format(EbayArticleConstants.EbayTitleWinterWheelTemplate, wheel.Description);
                }
            }
            
            return article.Description;
        }

        public void RemoveAllEbayArticles(IArticle articleToDelete)
        {
            facade.DeleteEbayItem(articleToDelete.EbayId);
            facade.DeleteEbayItem(articleToDelete.EbayId2);
            facade.DeleteEbayItem(articleToDelete.EbayId4);
        }
    }
}
