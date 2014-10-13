using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EbaySeller.Common.DataInterface;
using EbaySeller.Common.Helper;
using EbaySeller.Model.Source.CSV.Reader;
using EbaySeller.Model.Source.CSV.Writer;
using EbaySeller.Model.Source.Images;
using EbaySeller.ViewModel.Source.Import;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;

namespace EbaySeller.ViewModel.Source.Gambio
{
    public class GambioViewModel:ViewModelBase
    {
        private bool isUploadingToEbay;
        private string priceMarge = "000";

        public bool IsUploadingToEbay
        {
            get { return isUploadingToEbay; }
            set
            {
                isUploadingToEbay = value;
                RaisePropertyChanged("IsUploadingToEbay");
            }
        } 

        public string PriceMarge
        {
            get { return priceMarge; }
            set
            {
                priceMarge = value;
                RaisePropertyChanged("PriceMarge");
            }
        }

        public RelayCommand ExportCsvDataCommand
        {
            get{ return new RelayCommand(ExportCsvData);}
        }

        public RelayCommand ImageImportCommand
        {
            get{ return new RelayCommand(ImportImages);}
        }

        private void ExportCsvData()
        {
            BackgroundWorker bw = new BackgroundWorker();

             var fileDialog = new SaveFileDialog();
            if (fileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            try
            {
                bw.DoWork += delegate
                    {
                        IFilterableViewModel viewModel = GetViewModelFromLocator();

                        var list = viewModel.GetListsToFilter();
                        var mainList = list[ImportViewModel.BaseArticleKey];
                        double doubleValue;
                        if (!double.TryParse(priceMarge, out doubleValue))
                        {
                            MessageBox.Show("Ungültiger Margenwert: " + priceMarge);
                            return;
                        }
                        var gambioCSVWriter = new GambioArticleCSVWriter(fileDialog.FileName, doubleValue);
                        gambioCSVWriter.WriteToCSVFile(mainList);
                    };
                bw.RunWorkerAsync();
            }
            catch (Exception exception)
            {
                
            }
        }

        private void ImportImages()
        {
            IFilterableViewModel viewModel = GetViewModelFromLocator();
            var mainList = viewModel.GetFilteredList();
            var fileDialog = new FolderBrowserDialog();
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                var imageImporter = new ImageLoader(fileDialog.SelectedPath);
                foreach(var article in mainList)
                {
                    try
                    {
                        var imageName = article.ArticleId;
                        imageImporter.SaveImageToFile(article.ImageLink, imageName + ".jpg");
                    }
                    catch (Exception exception)
                    {
                        
                        
                    }
                }
            }
            
        }

        private IFilterableViewModel GetViewModelFromLocator()
        {
            return SimpleIoc.Default.GetInstance<ImportViewModel>();
        }
    }
}
