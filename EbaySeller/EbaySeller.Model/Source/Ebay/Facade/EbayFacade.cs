using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using EbaySeller.Model.Source.Ebay.Interfaces;
using eBay.Service.Call;
using eBay.Service.Core.Sdk;
using eBay.Service.Core.Soap;
using eBay.Service.Util;
using log4net;

namespace EbaySeller.Model.Source.Ebay.Facade
{
    public class EbayFacade: IEbayFacade
    {
        private ApiContext Context;
        private AddFixedPriceItemCall api2call;
        private ReviseFixedPriceItemCall reviseItemCall;
        private EndItemCall deleteItemCall;
        private ILog logger = LogManager.GetLogger(typeof(EbayUploader));

        public EbayFacade()
        {
            InitializeContext();
            api2call = new AddFixedPriceItemCall(Context);
            reviseItemCall = new ReviseFixedPriceItemCall(Context);
            deleteItemCall = new EndItemCall(Context);
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

            Context.ApiLogManager = new ApiLogManager();
            Context.ApiLogManager.ApiLoggerList.Add(new FileLogger(
                                                        ConfigurationManager.AppSettings["Ebay.Logfile"], false,
                                                        false, true));
            Context.ApiLogManager.EnableLogging = true;
        }

        public string AddFixedPriceItem(ItemType ebayItemToCreate)
        {
            var addFixedPriceDelegate = new Func<string>(() =>
                {
                    api2call.PictureFileList = new StringCollection();
                    api2call.AddFixedPriceItem(ebayItemToCreate);
                    return ebayItemToCreate.ItemID;
                });
            return ExecuteWithExceptionHandling(addFixedPriceDelegate, ebayItemToCreate.ItemID);
        }

        public void DeleteEbayItem(string ebayId)
        {
            if (string.IsNullOrEmpty(ebayId))
            {
                return;
            }
            var deleteEbayItem = new Func<string>(() =>
            {
                deleteItemCall.EndItem(ebayId, EndReasonCodeType.NotAvailable, "");
                return ebayId;
            });
            ExecuteWithExceptionHandling(deleteEbayItem, ebayId);
        }

        public void ReviseEbayArticle(ItemType itemToRevise)
        {
            var reviseItemCallFunction = new Func<string>(() =>
            {
                reviseItemCall.ReviseFixedPriceItem(itemToRevise, new StringCollection());
                return itemToRevise.ItemID;
            });
            ExecuteWithExceptionHandling(reviseItemCallFunction, itemToRevise.ItemID);
        }

        private string ExecuteWithExceptionHandling(Func<string> callToExcecute, string itemId)
        {
            try
            {
                return callToExcecute.Invoke();
            }
            catch (ApiException apiException)
            {
                logger.Warn(string.Format("Fehler bei EbayApiCall Artikel: {0}", itemId), apiException);
            }
            return itemId;
        }
    }
}
