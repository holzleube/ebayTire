using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using EbaySeller.Model.Source.Exceptions;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using eBay.Service.Core.Sdk;

namespace EbaySeller.ViewModel.Source.Import
{
    public class WheelDetailListViewModel: ViewModelBase
    {
        private ICollectionView wheelList = new ListCollectionView(new List<IWheel>());

        private int countOfWheels;
        private List<IArticle> wheelListFlat;

        private bool isUploadingToEbay;
        private string ebayArticlePercentage = string.Empty;
        private string ebayArticleAmount = string.Empty;

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
            var ebayUploader = new EbayUploader();
            double percentage = 0.0;
            double amount = 0.0;
            if (!IsValidDouble(EbayArticlePercentage, out percentage) && !IsValidDouble(EbayArticleAmount, out amount))
            {
                MessageBox.Show("Bitte entweder einen Betrag oder einen Prozentsatz für die Preiskalkulation des Artikels angeben.");
                return;
            }
             
            percentage = percentage/100 + 1;
            var saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                IsUploadingToEbay = true;
                BackgroundWorker bw = new BackgroundWorker();

                try
                {
                    bw.DoWork += delegate
                    {
                        var ebayResults = ebayUploader.RefreshOrCreateEbayArticle(WheelListFlat, saveFileDialog.FileName, percentage, amount);
                        var ebayArticleCsvWriter = new EbayArticleCSVWriter(saveFileDialog.FileName);
                        ImportViewModel importViewModel = SimpleIoc.Default.GetInstance<ImportViewModel>();
                        var dictionary = importViewModel.OriginalArticles;
                        foreach (var ebayResult in ebayResults)
                        {
                            if (dictionary.ContainsKey(ebayResult.ArticleId))
                            {
                                dictionary.Remove(ebayResult.ArticleId);
                            }
                            dictionary[ebayResult.ArticleId] = ebayResult;
                        }
                        foreach (var article in dictionary.Values)
                        {
                            ebayArticleCsvWriter.WriteToCSVFile(article);
                        }
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
