using System;
using System.Collections.Generic;
using System.Configuration;
using EbaySeller.Common.DataInterface;
using EbaySeller.Model.Source.CSV.Writer;
using EbaySeller.Model.Source.Ebay.Constants;
using EbaySeller.Model.Source.Ebay.Interfaces;
using EbaySeller.Model.Source.Ebay.Template;
using eBay.Service.Call;
using eBay.Service.Core.Sdk;
using eBay.Service.Core.Soap;
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

        private ApiContext Context;
        private AddFixedPriceItemCall api2call;
        private ReviseFixedPriceItemCall reviseItemCall;
        private double currentAmount;
        private string currentMail;
        private EndItemCall deleteItemCall;

        public EbayUploader()
        {
            InitializeContext();
            api2call = new AddFixedPriceItemCall(Context);
            reviseItemCall = new ReviseFixedPriceItemCall(Context);
            deleteItemCall = new EndItemCall(Context);
            currentMail = ConfigurationManager.AppSettings["Paypal.Mail"];
        }

        public IArticle RefreshOrCreateEbayArticle(IArticle article, EbayArticleCSVWriter cswWriter, double amount, string template)
        {
            IArticle newArticle = null;
            currentAmount = amount;
            
            if (string.IsNullOrEmpty(article.EbayId))
            {
                if (article.Availability < EbayArticleConstants.MinimumCountOfArticles)
                {
                    return article;
                }
                newArticle = LoadUpNewSingleArticle(article, template);
            }
            else
            {
                if (article.IsToDelete)
                {
                    RemoveItem(article);
                    
                    return null;
                }
                newArticle = ReviseEbayArticle(article);
            }

            cswWriter.WriteToCSVFile(newArticle);
            return newArticle;
        }

        private IArticle ReviseEbayArticle(IArticle article)
        {
            var ebayType = new ItemType();
            
            ebayType.ItemID = article.EbayId;
            ebayType.QuantityAvailable = GetQuantityOfArticle(article);
            ebayType.StartPrice = GetCalculatedPrice(article);
            ebayType.VATDetails = GetVatDetails();
            ebayType.Storefront = GetStorefrontType();
            var wheel = article as IWheel;
            if (wheel != null)
            {
                ebayType.ItemSpecifics = GetItemSpecifics(wheel);
            }

            reviseItemCall.DeletedFieldList = new StringCollection();
            reviseItemCall.ReviseFixedPriceItem(ebayType, new StringCollection());
            
            return article;
        }

        private static int GetQuantityOfArticle(IArticle article)
        {
            if (article.Availability > 10)
            {
                return 6;
            }
            return article.Availability;
        }

        private IArticle LoadUpNewSingleArticle(IArticle article, string template)
        {
            var ebayType = new ItemType();

            var wheel = article as IWheel;
            if (wheel != null)
            {
                ebayType.Title = GetTitleFromArticle(wheel);
                ebayType.ItemSpecifics = GetItemSpecifics(wheel);
            }
            ebayType.Description = GetDescriptionFromArticle(wheel, template);

            ebayType.ListingType = ListingTypeCodeType.StoresFixedPrice;
            ebayType.ListingDuration = "GTC";

            ebayType.Currency = CurrencyCodeType.EUR;
            ebayType.StartPrice = GetCalculatedPrice(article);

            ebayType.Location = "Baden-Baden";
            ebayType.Country = CountryCodeType.DE;

            var category = new CategoryType();
            category.CategoryID = "9891";
            ebayType.PrimaryCategory = category;

            ebayType.Quantity = GetQuantityOfArticle(article);

            ebayType.ConditionID = 1000;

            ebayType.PaymentMethods = new BuyerPaymentMethodCodeTypeCollection(PaymentMethods);
            ebayType.PayPalEmailAddress = currentMail;
            ebayType.DispatchTimeMax = 1;

            ebayType.ShippingDetails = GetShippingDetails();
            
            ebayType.MotorsGermanySearchable = true;
            ebayType.VATDetails = GetVatDetails();

            ebayType.Storefront = GetStorefrontType();

            ebayType.ReturnPolicy = GetPolicy();
            api2call.PictureFileList = new StringCollection();
            ebayType.PictureDetails = GetPictureDetails(article);
            
            api2call.AddFixedPriceItem(ebayType);
            
            article.EbayId = ebayType.ItemID;
            return article;
        }

        private StorefrontType GetStorefrontType()
        {
            return new StorefrontType {StoreCategoryID = 3492568016};
        }

        private NameValueListTypeCollection GetItemSpecifics(IWheel wheel)
        {
            var result = new NameValueListTypeCollection();
            result.Add(GetSingleItemSpecific("Reifenhersteller", wheel.Manufactorer));
            result.Add(GetSingleItemSpecific("Hersteller-Artikelnummer", wheel.ArticleId));
            result.Add(GetSingleItemSpecific("Reifenspezifikation", GetWheelSpecification(wheel)));
            result.Add(GetSingleItemSpecific("Reifenbreite", wheel.WheelWidth.ToString()));
            result.Add(GetSingleItemSpecific("Reifenquerschnitt", wheel.WheelHeight.ToString()));
            result.Add(GetSingleItemSpecific("Zollgröße", wheel.CrossSection.Replace('R', ' ')));
            result.Add(GetSingleItemSpecific("Tragfähigkeitsindex", wheel.WeightIndex.ToString()));
            result.Add(GetSingleItemSpecific("Geschwindigkeitsindex", wheel.SpeedIndex.ToString()));
            result.Add(GetSingleItemSpecific("Winterreifen (Ja/Nein)", wheel.IsWinter ? "Ja":"Nein"));
            return result;
        }

        private string GetWheelSpecification(IWheel wheel)
        {
            if (wheel.IsWinter)
            {
                return "Winterreifen";
            }
            if (wheel.IsMudSnow)
            {
                return "Ganzjahresreifen";
            }
            return "Sommerreifen";
        }

        private NameValueListType GetSingleItemSpecific(string key, string value)
        {
            NameValueListType nv1 = new NameValueListType();
            nv1.Name = key;
            StringCollection nv1Col = new StringCollection();
            nv1Col.Add(value);
            nv1.Value = nv1Col;
            return nv1;
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

            //double newPrice = article.Price + currentAmount + 0.35;
            //double calculated = (newPrice + (newPrice / 0.95) * 0.05) * 1.19;
            return new AmountType {currencyID = CurrencyCodeType.EUR, Value = price};
        }


        private void InitializeContext()
        {
            Context = new ApiContext();
            Context.SoapApiServerUrl = ConfigurationManager.AppSettings["Environment.ApiServerUrl"];
            ApiCredential apiCredential = new ApiCredential();
            
            apiCredential.eBayToken =
                ConfigurationManager.AppSettings["UserAccount.ApiToken"];
            apiCredential.ApiAccount = new ApiAccount();
            apiCredential.ApiAccount.Application = ConfigurationManager.AppSettings["Environment.AppId"];
            apiCredential.ApiAccount.Certificate = ConfigurationManager.AppSettings["Environment.CertId"];
            apiCredential.ApiAccount.Developer = ConfigurationManager.AppSettings["Environment.DevId"];
            Context.ApiCredential = apiCredential;
            Context.Site = SiteCodeType.Germany;
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

        public void RemoveItem(IArticle articleToDelete)
        {
            deleteItemCall.EndItem(articleToDelete.EbayId, EndReasonCodeType.NotAvailable, "");
        }
    }
}
