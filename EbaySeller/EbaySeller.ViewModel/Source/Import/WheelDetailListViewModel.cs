using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using EbaySeller.Model.Source.Data.Interfaces;
using EbaySeller.Model.Source.Ebay;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using eBay.Service.Core.Sdk;

namespace EbaySeller.ViewModel.Source.Import
{
    public class WheelDetailListViewModel: ViewModelBase
    {
        private ICollectionView wheelList = new ListCollectionView(new List<IWheel>());

        private int countOfWheels;
        private List<IArticle> wheelListFlat;

        private ICommand uploadToEbayCommand;

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

        public string CountOfData
        {
            get { return "Daten: " + countOfWheels; }
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
            var saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ebayUploader.RefreshOrCreateEbayArticle(WheelListFlat, saveFileDialog.FileName);
                }
                catch (ApiException e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }
    }
}
