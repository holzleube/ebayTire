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
using EbaySeller.Model.Source.CSV.Reader;
using EbaySeller.Model.Source.CSV.Writer;
using EbaySeller.Model.Source.Data.Interfaces;
using EbaySeller.Model.Source.Ebay;
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

        public ICommand UploadToEbayCommand
        {
            get
            {
                return new RelayCommand(UploadCurrentListToEbay);
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
            double amount = 0.0;
            if (!IsValidDouble(EbayArticleAmount, out amount))
            {
                MessageBox.Show("Bitte geben Sie einen gültigen Gewinn ein.");
                return;
            }
             
            var saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                IsUploadingToEbay = true;
                BackgroundWorker bw = new BackgroundWorker();

                try
                {
                    bw.DoWork += delegate
                    {
                        string template = TemplateLoader.LoadTemplateFromUri(ConfigurationManager.AppSettings["Ebay.Template"]);
                        var ebaySingleArticleCsvWriter = new EbayArticleCSVWriter(saveFileDialog.FileName + ".diffs");
                        ImportViewModel importViewModel = SimpleIoc.Default.GetInstance<ImportViewModel>();
                        CountOfCurrentUploadedItems = 0;
                        var allArticles = IterateThroughAllItemsAndUploadThem(importViewModel.OriginalArticles, ebaySingleArticleCsvWriter, amount, template);
                        WriteAllArticlesBackToCSV(saveFileDialog.FileName, allArticles);
                        IsUploadingToEbay = false;
                    };
                    bw.RunWorkerAsync();

                }
                catch (Exception e)
                {
                    MessageBox.Show("Es ist folgender Fehler aufgetreten: " + e.Message);
                }
            }
        }

        private Dictionary<string, IArticle>.ValueCollection IterateThroughAllItemsAndUploadThem(Dictionary<string, IArticle> dictionary,
                                                               EbayArticleCSVWriter ebaySingleArticleCsvWriter, double amount,
                                                               string template)
        {
            var ebayUploader = new EbayUploader();
            foreach (var articleToUpload in WheelListFlat)
            {
                IArticle result = null;
                try
                {
                    result = ebayUploader.RefreshOrCreateEbayArticle(articleToUpload,
                                                                     ebaySingleArticleCsvWriter, amount,
                                                                     template);
                    if (dictionary.ContainsKey(result.ArticleId))
                    {
                        dictionary.Remove(result.ArticleId);
                    }
                    dictionary[result.ArticleId] = result;
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

        private static void WriteAllArticlesBackToCSV(string filename, IEnumerable<IArticle> articles)
        {
            var ebayArticleCsvWriter = new EbayArticleCSVWriter(filename);
            foreach (var article in articles)
            {
                ebayArticleCsvWriter.WriteToCSVFile(article);
            }
        }

        private bool IsValidDouble(string stringToCheck, out double variable)
        {
            variable = 0.0;
            if (stringToCheck.Equals(string.Empty))
            {
                return false;
            }
            try
            {
                variable = double.Parse(stringToCheck);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
