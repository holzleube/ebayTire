using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using EbaySeller.Model.Source.Data.Interfaces;
using GalaSoft.MvvmLight;

namespace EbaySeller.ViewModel.Source.Import
{
    public class WheelDetailListViewModel: ViewModelBase
    {
        private ICollectionView wheelList = new ListCollectionView(new List<IWheel>());

        private int countOfWheels = 0;
        private List<IArticle> wheelListFlat;


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
    }
}
