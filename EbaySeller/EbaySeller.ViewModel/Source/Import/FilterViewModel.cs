using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Navigation;
using EbaySeller.Common.DataInterface;
using EbaySeller.ViewModel.Source.Filter;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;

namespace EbaySeller.ViewModel.Source.Import
{
    public class FilterViewModel: ViewModelBase
    {
        private List<IFilterCriteria<IWheel>> activeFilters;
        private Dictionary<string, IFilterCriteria<IWheel>> availableFilterDictionary;
        private bool isWheelFilterChecked;
        private bool isDotFilterChecked;
        private bool isWinterFilterChecked;
        private bool isSummerFilterChecked;
        private bool isWidthHeightFilterChecked;
        private bool allFilterChecked;
        private bool isEbayArticleFilterChecked;
        private bool isNewArticleFilterChecked;
        private bool isAllDotsFilterChecked;
        private bool isCrossSectionFilterChecked;

        public FilterViewModel()
        {
            activeFilters = new List<IFilterCriteria<IWheel>>();
            availableFilterDictionary = new Dictionary<string, IFilterCriteria<IWheel>>();
            availableFilterDictionary.Add(CarWheelFilterKey, new CarFilterCriteria());
            availableFilterDictionary.Add(DotFilterKey, new DotFilter());
            availableFilterDictionary.Add(WinterFilterKey, new WinterFilterCriteria());
            availableFilterDictionary.Add(SummerFilterKey, new SummerFilterCriteria());
            availableFilterDictionary.Add(WidthHeightFilterKey, new WidthHeightFilter());
            availableFilterDictionary.Add(EbayArticleFilterKey, new EbayArticleFilter());
            availableFilterDictionary.Add(NewArticleFilterKey, new NewArticleFilter());
            availableFilterDictionary.Add(AllDotsFilterKey, new AllDotsFilter());
            availableFilterDictionary.Add(CrossSectionFilterKey, new CrossSectionFilter());
        }

        #region keys
        private const string CarWheelFilterKey = "CarWheelFilterKey";
        private const string DotFilterKey = "DotFilterKey";
        private const string WinterFilterKey = "WinterFilterKey";
        private const string SummerFilterKey = "SummerFilterKey";
        private const string WidthHeightFilterKey = "WidthHeightFilterKey";
        private const string EbayArticleFilterKey = "EbayArticleFilterKey";
        private const string NewArticleFilterKey = "NewArticleFilterKey";
        private const string AllDotsFilterKey = "AllDotsFilterKey";
        private const string CrossSectionFilterKey = "CrossSectionFilterKey";
        #endregion

        #region boolProperties

        public bool CarWheelFilterChecked
        {
            get { return isWheelFilterChecked; }
            set
            {
                UpdateFilter(CarWheelFilterKey);
                isWheelFilterChecked = value;
                RaisePropertyChanged("CarWheelFilterChecked");
            }
        }

        public bool AllDotsChecked
        {
            get { return isAllDotsFilterChecked; }
            set
            {
                UpdateFilter(AllDotsFilterKey);
                isAllDotsFilterChecked = value;
                RaisePropertyChanged("AllDotsChecked");
            }
        }

        public bool EbayArticleFilterChecked
        {
            get { return isEbayArticleFilterChecked; }
            set
            {
                UpdateFilter(EbayArticleFilterKey);
                isEbayArticleFilterChecked = value;
                RaisePropertyChanged("EbayArticleFilterChecked");
            }
        }

        public bool CrossSectionFilterChecked
        {
            get { return isCrossSectionFilterChecked; }
            set
            {
                UpdateFilter(CrossSectionFilterKey);
                isCrossSectionFilterChecked = value;
                RaisePropertyChanged("CrossSectionFilterChecked");
            }
        }

        public bool NewArticleFilterChecked
        {
            get { return isNewArticleFilterChecked; }
            set
            {
                UpdateFilter(NewArticleFilterKey);
                isNewArticleFilterChecked = value;
                RaisePropertyChanged("NewArticleFilterChecked");
            }
        }

        public bool AllFilterChecked
        {
            get { return allFilterChecked; }
            set
            {
                activeFilters = new List<IFilterCriteria<IWheel>>();
                if (value)
                {
                    activeFilters.Add(availableFilterDictionary[CarWheelFilterKey]);
                    activeFilters.Add(availableFilterDictionary[WinterFilterKey]);
                    activeFilters.Add(availableFilterDictionary[DotFilterKey]);
                    activeFilters.Add(availableFilterDictionary[WidthHeightFilterKey]);
                }
                allFilterChecked = value;
                RaisePropertyChanged("AllFilterChecked");
            }
        }

        public bool DotFilterChecked
        {
            get { return isDotFilterChecked; }
            set
            {
                UpdateFilter(DotFilterKey);
                isDotFilterChecked = value;
                RaisePropertyChanged("DotFilterChecked");
            }
        }

        public bool WinterFilterChecked
        {
            get { return isWinterFilterChecked; }
            set
            {
                UpdateFilter(WinterFilterKey);
                isWinterFilterChecked = value;
                RaisePropertyChanged("WinterFilterChecked");
            }
        }

        public bool SummerFilterChecked
        {
            get { return isSummerFilterChecked; }
            set
            {
                UpdateFilter(SummerFilterKey);
                isSummerFilterChecked = value;
                RaisePropertyChanged("SummerFilterChecked");
            }
        }

        public bool WidthHeightFilterChecked
        {
            get { return isWidthHeightFilterChecked; }
            set
            {
                UpdateFilter(WidthHeightFilterKey);
                isWidthHeightFilterChecked = value;
                RaisePropertyChanged("WidthHeightFilterChecked");
            }
        }

        private void UpdateFilter(string filterKey)
        {
            var filterCriteria = availableFilterDictionary[filterKey];
            if (activeFilters.Contains(filterCriteria))
            {
                activeFilters.Remove(filterCriteria);
                return;
            }
            activeFilters.Add(filterCriteria);
        }

        #endregion

        public RelayCommand FilterToggleCommand
        {
            get
            {
                return new RelayCommand(WheelFilterWasPressed);
            }
        }

        private void WheelFilterWasPressed()
        {
            IFilterableViewModel viewModel = GetViewModelFromLocator();
            var listsToFilter = viewModel.GetListsToFilter();
            if (activeFilters.Count == 0)
            {
                viewModel.SetFilteredLists(listsToFilter);
            }

            var results = new Dictionary<string, List<IArticle>>();
            foreach (var listToFilter in listsToFilter)
            {
                var filteredList = GetFilteredList(listToFilter);
                results.Add(listToFilter.Key, filteredList);
            }
            viewModel.SetFilteredLists(results);
        }

        private IFilterableViewModel GetViewModelFromLocator()
        {
            return SimpleIoc.Default.GetInstance<ImportViewModel>();
        }

        private List<IArticle> GetFilteredList(KeyValuePair<string, List<IArticle>> listToFilter)
        {
            var filteredList = new List<IArticle>();
            foreach (var article in listToFilter.Value)
            {
                var wheel = article as IWheel;
                if (wheel == null)
                {
                    continue;
                }
                var isInFilter = activeFilters.All(filterCriteria => filterCriteria.IsInFilter(wheel));
                if (isInFilter)
                {
                    filteredList.Add(wheel);
                }
            }
            return filteredList;
        }

    }
}
