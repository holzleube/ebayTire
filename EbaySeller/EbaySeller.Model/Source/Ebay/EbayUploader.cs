using System;
using System.Collections.Generic;
using System.Configuration;
using EbaySeller.Model.Source.CSV.Writer;
using EbaySeller.Model.Source.Data.Interfaces;
using EbaySeller.Model.Source.Ebay.Interfaces;
using eBay.Service.Call;
using eBay.Service.Core.Sdk;
using eBay.Service.Core.Soap;

namespace EbaySeller.Model.Source.Ebay
{
    public class EbayUploader:IEbayUploader
    {
        private static readonly BuyerPaymentMethodCodeType[] PaymentMethods = new[]
            {
                BuyerPaymentMethodCodeType.CashOnPickup, 
                BuyerPaymentMethodCodeType.PayPal,
                BuyerPaymentMethodCodeType.StandardPayment
            };

        private ApiContext Context;

        public void RefreshOrCreateEbayArticle(List<IArticle> articlesToRefresh, string fileName)
        {
            var ebayArticleCsvWriter = new EbayArticleCSVWriter(fileName);
            InitializeContext();
            var apiCall = new AddItemCall(Context);
            string template = "";
            foreach (var article in articlesToRefresh)
            {
                var ebayType = new ItemType();
                ebayType.Currency = CurrencyCodeType.EUR;
                ebayType.Country = CountryCodeType.DE;
                ebayType.PaymentMethods = new BuyerPaymentMethodCodeTypeCollection(PaymentMethods);

                ebayType.Title = GetTitleFromArticle(article);
                ebayType.Description = GetDescriptionFromArticle(article, template);
                ebayType.Quantity = article.Availability;
                ebayType.Location = "Baden-Baden";
                
                var prodId = new ExternalProductIDType {Value = article.ArticleId, Type = ExternalProductCodeType.ProductID};
                ebayType.ExternalProductID = prodId;
                ebayType.ListingDuration = "GTC";
                ebayType.PrimaryCategory = new CategoryType {CategoryID = "66471"};
                ebayType.BuyItNowPrice = new AmountType {currencyID = ebayType.Currency, Value = article.Price};

                ebayType.ConditionID = 1000;
                ebayType.ShippingDetails = GetShippingDetails();
                //ebayType.ReturnPolicy = GetPolicy();
                try
                {
                    apiCall.AddItem(ebayType);
                }
                catch (Exception e)
                {
                    
                }
                article.EbayId = ebayType.ItemID;
                ebayArticleCsvWriter.WriteToCSVFile(article);
                break;

            }
            
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
            sd.PaymentInstructions = "Zahlungsanweisungen optional";
            sd.ShippingType = ShippingTypeCodeType.Flat;

            ShippingServiceOptionsType st1 = new ShippingServiceOptionsType();
            st1.ShippingService = ShippingServiceCodeType.DE_SpecialDelivery.ToString();
            st1.ShippingServiceAdditionalCost = new AmountType() { Value = 7.3, currencyID = CurrencyCodeType.EUR };

            st1.ShippingServiceCost = new AmountType() { Value = 6.9, currencyID = CurrencyCodeType.EUR };
            st1.ShippingServicePriority = 1;
            st1.ShippingInsuranceCost = new AmountType() { Value = 16.9, currencyID = CurrencyCodeType.EUR };

            sd.ShippingServiceOptions = new ShippingServiceOptionsTypeCollection(new[] { st1});
            return sd;
        }

        public static ReturnPolicyType GetPolicy()
        {
            ReturnPolicyType policy = new ReturnPolicyType();
            policy.Description = "Verbraucher können den Artikel zu den unten angegebenen Bedingungen zurückgeben";
            policy.ReturnsWithinOption = "Days_14";
            policy.ReturnsAcceptedOption = "ReturnsAccepted";
            policy.ShippingCostPaidByOption = "Buyer";
            return policy;
        }

        private string GetDescriptionFromArticle(IArticle article, string template)
        {
            return "Meine Beschreibung";
        }

        private string GetTitleFromArticle(IArticle article)
        {
            return article.Description;
        }
    }
}
