using System;
using System.Collections.Generic;
using System.Configuration;
using EbaySeller.Model.Source.CSV.Writer;
using EbaySeller.Model.Source.Data.Interfaces;
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
                //BuyerPaymentMethodCodeType.MoneyXferAccepted, 
                BuyerPaymentMethodCodeType.PayPal
            };

        private ApiContext Context;

        public void RefreshOrCreateEbayArticle(List<IArticle> articlesToRefresh, string fileName)
        {

            var ebayArticleCsvWriter = new EbayArticleCSVWriter(fileName);
            InitializeContext();
            var apiCall = new AddItemCall(Context);
            var api2call = new AddFixedPriceItemCall(Context);
            //var categoryCall = new GetCategoryFeaturesCall(Context);
            //categoryCall.DetailLevelList.Add(DetailLevelCodeType.ReturnAll);
            //categoryCall.ViewAllNodes = true;
            //categoryCall.CategoryID = "9891";
            //var test = categoryCall.GetCategoryFeatures();
            string template = TemplateLoader.LoadTemplateFromUri(ConfigurationManager.AppSettings["Ebay.Template"]);
            LodUpSingleArticle(articlesToRefresh[119], template, api2call, ebayArticleCsvWriter);
            //foreach (var article in articlesToRefresh)
            //{
            //    LodUpSingleArticle(article, template, api2call, ebayArticleCsvWriter);
            //    break;
            //}
        }

        private void LodUpSingleArticle(IArticle article, string template, AddFixedPriceItemCall api2call,
                                        EbayArticleCSVWriter ebayArticleCsvWriter)
        {
            var ebayType = new ItemType();

            var wheel = article as IWheel;
            if (wheel != null)
            {
                ebayType.ItemSpecifics = AddItemSpecific(wheel);
                ebayType.Title = GetTitleFromArticle(wheel);
            }
            ebayType.Description = GetDescriptionFromArticle(wheel, template);

            ebayType.ListingType = ListingTypeCodeType.FixedPriceItem;
            ebayType.ListingDuration = "Days_7";

            ebayType.Currency = CurrencyCodeType.EUR;
            ebayType.StartPrice = new AmountType {currencyID = CurrencyCodeType.EUR, Value = article.Price};

            ebayType.Location = "Baden-Baden";
            ebayType.Country = CountryCodeType.DE;

            var category = new CategoryType();
            //category.CategoryID = "34639";
            category.CategoryID = "9891";
            ebayType.PrimaryCategory = category;

            ebayType.Quantity = article.Availability;

            ebayType.ConditionID = 1000;

            ebayType.PaymentMethods = new BuyerPaymentMethodCodeTypeCollection(PaymentMethods);
            ebayType.PayPalEmailAddress = "dane160290@yahoo.de";
            ebayType.DispatchTimeMax = 1;

            ebayType.ShippingDetails = GetShippingDetails();
            
            ebayType.MotorsGermanySearchable = true;

            //var prodId = new ExternalProductIDType {Value = article.ArticleId, Type = ExternalProductCodeType.ProductID};
            //ebayType.ExternalProductID = prodId;

            ebayType.ReturnPolicy = GetPolicy();
            api2call.PictureFileList = new StringCollection();
            ebayType.PictureDetails = new PictureDetailsType();
            ebayType.PictureDetails.PictureURL = new StringCollection();
            ebayType.PictureDetails.PictureURL.Add(article.ImageLink);
            ebayType.PictureDetails.PhotoDisplay = PhotoDisplayCodeType.None;

            api2call.AddFixedPriceItem(ebayType);
            
            article.EbayId = ebayType.ItemID;
            ebayArticleCsvWriter.WriteToCSVFile(article);
        }

        private NameValueListTypeCollection AddItemSpecific(IWheel article)
        {
            var valueSpecifics = new NameValueListTypeCollection();

            return valueSpecifics;

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
            
            sd.InsuranceFee = new AmountType(){Value = 2.8, currencyID = CurrencyCodeType.EUR};
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
    }
}
