using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using EbaySeller.Common.DataInterface;
using EbaySeller.Common.Helper;
using EbaySeller.Model.Source.CSV.Reader;
using EbaySeller.Model.Source.CSV.Writer;
using EbaySeller.Model.Source.Ebay;
using EbaySeller.Model.Source.Ebay.Interfaces;
using EbaySeller.Model.Source.Ebay.Template;
using EbaySeller.Model.Source.Exceptions;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using eBay.Service.Core.Sdk;
using log4net;

namespace EbaySeller.ViewModel.Source.Import
{
    public class WheelDetailListViewModel: ViewModelBase
    {
        private ICollectionView wheelList = new ListCollectionView(new List<IWheel>());
        private ILog logger = LogManager.GetLogger(typeof(EbayUploader));
        private int countOfWheels;
        private List<IArticle> wheelListFlat;

        private bool isUploadingToEbay;
        private string ebayArticlePercentage = string.Empty;
        private string ebayArticleAmount = string.Empty;
        private int countOfCurrentUploadedItems;
        private List<IArticle> articlesToDeleteList;
        private static string baseFileName;
        private IEbayUploader ebayUploader;

        public WheelDetailListViewModel()
        {
            baseFileName = ConfigurationManager.AppSettings["Ebay.BaseFile"];
            ebayUploader = new EbayUploader();
        }

        public ICommand UploadToEbayCommand
        {
            get
            {
                return new RelayCommand(UploadCurrentListToEbay);
            }
        }

        public ICommand DeleteArticleEbayCommand
        {
            get
            {
                return new RelayCommand(DeleteEbayArticles);
            }
        }

        public ICollectionView WheelList
        {
            get { return wheelList; }
            set
            {
                wheelList = value;
                RaisePropertyChanged("WheelList");
                RaisePropertyChanged("CountOfData");
            }
        }
        
        public List<IArticle> WheelListFlat
        {
            get { return wheelListFlat; }
            set
            {
                wheelListFlat = value;
                RaisePropertyChanged("WheelListFlat");
                RaisePropertyChanged("CountOfData");
            }
        }

        public string EbayArticlePercentage
        {
            get { return ebayArticlePercentage; }
            set
            {
                ebayArticlePercentage = value;
                RaisePropertyChanged("EbayArticlePercentage");
            }
        }

        public string EbayArticleAmount
        {
            get { return ebayArticleAmount; }
            set
            {
                ebayArticleAmount = value;
                RaisePropertyChanged("EbayArticleAmount");
            }
        }

        public string CountOfData
        {
            get { return "Daten: " + countOfWheels; }
        }

        public string LoadingState
        {
            get { return string.Format(ViewConstants.UploadingItemsConstant, CountOfCurrentUploadedItems, countOfWheels); }
        }

        public int CountOfCurrentUploadedItems
        {
            get { return countOfCurrentUploadedItems; }
            set 
            { 
                countOfCurrentUploadedItems = value;
                RaisePropertyChanged("LoadingState");
            }
        }

        public bool IsUploadingToEbay
        {
            get { return isUploadingToEbay; }
            set
            {
                isUploadingToEbay = value;
                RaisePropertyChanged("IsUploadingToEbay");
            }
        } 

        public void InitGroupedWheelList(List<IArticle> wheels )
        {
            countOfWheels = wheels.Count;
            WheelListFlat = wheels;
           
            //WheelList = new ListCollectionView(wheels);
            //WheelList.GroupDescriptions.Add(new PropertyGroupDescription("Manufactorer"));
            //WheelList.SortDescriptions.Add(new SortDescription("Manufactorer", ListSortDirection.Ascending));
        }

        private void UploadCurrentListToEbay()
        {
            double amount;
            if (!DoubleChecker.IsValidDouble(EbayArticleAmount, out amount))
            {
                MessageBox.Show("Bitte geben Sie einen gültigen Gewinn ein. In der Form 4,5");
                return;
            }
            
            IsUploadingToEbay = true;
            var bw = new BackgroundWorker();

            try
            {
                bw.DoWork += delegate
                {
                    string template = TemplateLoader.LoadTemplateFromUri(ConfigurationManager.AppSettings["Ebay.Template"]);
                    var ebaySingleArticleCsvWriter = new EbayArticleCSVWriter(baseFileName + ".diffs");
                    CountOfCurrentUploadedItems = 0;
                    var allArticles = IterateThroughAllItemsAndUploadThem(GetOriginalArticles(), ebaySingleArticleCsvWriter, amount, template);
                    WriteAllArticlesBackToCSV(new List<IArticle>(allArticles));
                    IsUploadingToEbay = false;
                };
                bw.RunWorkerAsync();

            }
            catch (Exception e)
            {
                logger.Error("Schwerer Fehler beim Hochladen", e);
                MessageBox.Show("Es ist ein unbekannter Fehler beim Hochladen auf Ebay aufgetreten: " + e.Message);
            }
        }

        private Dictionary<string, IArticle> GetOriginalArticles()
        {
            ImportViewModel importViewModel = SimpleIoc.Default.GetInstance<ImportViewModel>();
            return importViewModel.OriginalArticles;
        } 

        private Dictionary<string, IArticle>.ValueCollection IterateThroughAllItemsAndUploadThem(Dictionary<string, IArticle> dictionary,
                                                               EbayArticleCSVWriter ebaySingleArticleCsvWriter, double amount,
                                                               string template)
        {
            foreach (var articleToUpload in WheelListFlat)
            {
                IArticle result = null;
                try
                {
                    result = ebayUploader.RefreshOrCreateEbayArticle(articleToUpload,
                                                                     ebaySingleArticleCsvWriter, amount,
                                                                     template);
                    var key = ArticleKeyGenerator.GetKeyFromArticle(articleToUpload);
                    if (dictionary.ContainsKey(key))
                    {
                        dictionary.Remove(key);
                    }
                    dictionary[key] = result;
                }
                catch (ApiException e)
                {
                    logger.Warn(
                        "Fehler bei Ebay Kommunikation von Datensatz ID:" + articleToUpload.ArticleId, e);
                }
                catch (Exception e)
                {
                    logger.Error("Unknown Exception On Uploading articles", e);
                    break;
                }
                CountOfCurrentUploadedItems++;
            }
            return dictionary.Values;
        }

        private static void WriteAllArticlesBackToCSV(List<IArticle> articles)
        {
            var ebayArticleCsvWriter = new EbayArticleCSVWriter(baseFileName);
            ebayArticleCsvWriter.WriteToCSVFile(articles);
        }

       

        private void DeleteEbayArticles()
        {
            string messageBoxText =string.Format("Sollen alle {0} Artikel gelöscht werden?", WheelListFlat.Count);
            string caption = "Lösche Artikel";
            var buttons = MessageBoxButtons.YesNo;
            var result = MessageBox.Show(messageBoxText, caption, buttons);
            if (result.Equals(DialogResult.Yes))
            {

                var allArticles = GetOriginalArticles();
                foreach (var articleToDelete in WheelListFlat)
                {
                    var key = articleToDelete.Description + articleToDelete.Description2;
                    ebayUploader.RemoveItem(articleToDelete);
                    allArticles.Remove(key);
                }
                WriteAllArticlesBackToCSV(new List<IArticle>(allArticles.Values));

            }
            
            
        }
    }
}
