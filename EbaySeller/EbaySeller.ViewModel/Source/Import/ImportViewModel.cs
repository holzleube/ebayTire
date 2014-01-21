using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Navigation;
using EbaySeller.Common.DataInterface;
using EbaySeller.Model.Source.CSV;
using EbaySeller.Model.Source.CSV.Reader;
using EbaySeller.Model.Source.Exceptions;
using EbaySeller.ViewModel.Source.Comperator;
using EbaySeller.ViewModel.Source.ViewInterfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using log4net;
using MessageBox = System.Windows.Forms.MessageBox;


namespace EbaySeller.ViewModel.Source.Import
{
    public class ImportViewModel: ViewModelBase, IFilterableViewModel
    {
        private List<IArticle> articles = new List<IArticle>();
        private List<IArticle> newArticles = new List<IArticle>();
        private Dictionary<string, IArticle> originalArticles = new Dictionary<string, IArticle>();
        private Dictionary<string, IArticle> newOriginalArticles = new Dictionary<string, IArticle>();

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private bool isLoadingBaseData = false;
        private bool isLoadingNewData = false;
        private const string BaseArticleKey = "BaseArticleKey";
        private const string NewArticleKey = "NewArticleKey";

        #region binding Data
        public bool IsLoadingBaseData
        {
            get { return isLoadingBaseData; }
            set 
            {
                isLoadingBaseData = value;
                RaisePropertyChanged("IsLoadingBaseData");
            }
        } 
        
        public bool IsLoadingNewData
        {
            get { return isLoadingNewData; }
            set 
            {
                isLoadingNewData = value;
                RaisePropertyChanged("IsLoadingNewData");
            }
        }

        public Dictionary<string, IArticle> OriginalArticles
        {
            get { return originalArticles; }
        } 

        public List<IArticle> Articles
        {
            get { return articles; }
            set
            {
                articles = value;
                RaisePropertyChanged("Articles");
                RaisePropertyChanged("CountOfData");
            }
        }
        public List<IArticle> NewArticles
        {
            get { return newArticles; }
            set
            {
                newArticles = value;
                RaisePropertyChanged("NewArticles");
                RaisePropertyChanged("CountOfNewData");
            }
        }

        
        public string CountOfData
        {
            get { return "Anzahl: " + Articles.Count; }
        }

        public string CountOfNewData
        {
            get { return "Anzahl: " + NewArticles.Count; }
        }

        #endregion
        
        #region commands

        public ICommand ImportRelayCommand
        {
            get
            {
                return new RelayCommand(ImportBaseDataWasClickedCommand);
            }
        }
        public ICommand ImportBaseFileRelayCommand
        {
            get
            {
                return new RelayCommand(ImportBaseFileWasClickedCommand);
            }
        }
        
        public RelayCommand ImportNewRelayCommand
        {
            get
            {
                return new RelayCommand(ImportNewBaseDataWasClickedCommand);
            }
        }
        
        public RelayCommand CompareBaseAndNewFileCommand
        {
            get
            {
                return new RelayCommand(CompareBaseAndNewFile);
            }
        }

        public RelayCommand CompareEbayArticlesCommand
        {
            get
            {
                return new RelayCommand(CompareEbayArticles);
            }
        }

        #endregion

        private void ImportBaseDataWasClickedCommand()
        {
            SetArticlesFromFileAsync(result =>
            {
                originalArticles = result;
                Articles = new List<IArticle>(result.Values);
                IsLoadingBaseData = false;
            }, () => IsLoadingBaseData = true);
        }
        
        private void ImportBaseFileWasClickedCommand()
        {
            string baseFileName = ConfigurationManager.AppSettings["Ebay.BaseFile"];
            StartBackgroundWorker(result =>
            {
                originalArticles = result;
                Articles = new List<IArticle>(result.Values);
                IsLoadingBaseData = false;
            }, () => IsLoadingBaseData = true, baseFileName);
        }

        private void ImportNewBaseDataWasClickedCommand()
        {
            SetArticlesFromFileAsync(result =>
                {
                    newOriginalArticles = result;
                    NewArticles = new List<IArticle>(result.Values);
                    IsLoadingNewData = false;
                }, () => IsLoadingNewData = true);
        }

        private void SetArticlesFromFileAsync(Action<Dictionary<string, IArticle>> afterLoad, Action beforeLoad  )
        {
            var fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                StartBackgroundWorker(afterLoad, beforeLoad, fileDialog.FileName);
            }
        }

        private static void StartBackgroundWorker(Action<Dictionary<string, IArticle>> afterLoad, Action beforeLoad, string fileName)
        {
            BackgroundWorker bw = new BackgroundWorker();

            try
            {
                bw.DoWork += delegate
                    {
                        beforeLoad.Invoke();
                        var reader = new CSVReader();
                        var resultArticles = reader.ReadArticlesFromFile(fileName);
                        afterLoad.Invoke(resultArticles);
                    };
                bw.RunWorkerAsync();
            }
            catch (FileNotReadyException ex)
            {
                MessageBox.Show("Datei kann nicht geöffnet werden, da sie bereits verwendet wird.");
            }
            catch (Exception e)
            {
                MessageBox.Show("Es ist folgender Fehler aufgetreten: " + e.Message);
            }
        }

        private void CompareBaseAndNewFile()
        {
            if (Articles.Count == 0)
            {
                MessageBox.Show("Keine Vergleichsdaten vorhanden. Bitte Datensätze laden. ");
                return;
            }
            
            if (NewArticles.Count == 0)
            {
                string messageBoxText = "Es sind keine Datensätze zum Vergleichen vorhanden. Wollen Sie alle Datensätze der Basis übernehmen?";
                string caption = "Leere Vergleichsdaten";
                var buttons = MessageBoxButtons.YesNo;
                var result = MessageBox.Show(messageBoxText,caption, buttons);
                switch (result)
                {
                        case DialogResult.Yes:
                            NavigateToWheelDetailListPage(Articles);
                        break;
                }
                return;
            }
            
            var copyOfNewArticles = new List<IArticle>(NewArticles);
            var resultList = new List<IArticle>();
            var comperator = new ArticleCrudComperator();
            foreach (var originalArticle in Articles)
            {
                try
                {
                    string key = originalArticle.Description + originalArticle.Description2;
                    var newArticle = newOriginalArticles[key];
                    if (!comperator.AreBothArticleEqual(originalArticle, newArticle))
                    {
                        var newArticleWithId = newArticle;
                        newArticleWithId.EbayId = originalArticle.EbayId;
                        resultList.Add(newArticleWithId);  
                    }
                    copyOfNewArticles.Remove(newArticle);
                }
                catch (KeyNotFoundException e)
                {
                    originalArticle.IsToDelete = true;
                    resultList.Add(originalArticle);  
                }
            }
            resultList.AddRange(copyOfNewArticles);
            NavigateToWheelDetailListPage(resultList);
        }

        private void CompareEbayArticles()
        {
            if (Articles.Count == 0 || NewArticles.Count == 0)
            {
                MessageBox.Show("Es kann kein Update gemacht werden, da entweder die Basis oder die Vergleichsdatei fehlt.");
            }
            var ebayArticles = Articles.Where(x => !string.IsNullOrEmpty(x.EbayId));
            var criteria = new PriceCriteria();
            var resultList = new List<IArticle>();
            foreach (var ebayArticle in ebayArticles)
            {
                var key = ebayArticle.Description + ebayArticle.Description2;
                try
                {
                    var newArticle = newOriginalArticles[key];
                    if (!criteria.IsCriteriaSatisfied(ebayArticle, newArticle))
                    {
                        newArticle.EbayId = ebayArticle.EbayId;
                        resultList.Add(newArticle);
                    }
                }
                catch (KeyNotFoundException)
                {
                    
                }
            }
            NavigateToWheelDetailListPage(resultList);
        }


        private static void NavigateToWheelDetailListPage(List<IArticle> resultList)
        {
            WheelDetailListViewModel wheelViewModel = SimpleIoc.Default.GetInstance<WheelDetailListViewModel>();
            wheelViewModel.InitGroupedWheelList(resultList);
            var targetPage = SimpleIoc.Default.GetInstance<IWheelDetailListPage>();
            SimpleIoc.Default.GetInstance<NavigationService>().Navigate(targetPage);
        }

        public Dictionary<string, List<IArticle>> GetListsToFilter()
        {
            var articleListsToFilter = new Dictionary<string, List<IArticle>>();
            articleListsToFilter.Add(BaseArticleKey, new List<IArticle>(originalArticles.Values));
            articleListsToFilter.Add(NewArticleKey, new List<IArticle>(newOriginalArticles.Values));
            return articleListsToFilter;
        }

        public void SetFilteredLists(Dictionary<string, List<IArticle>> filteredArticleLists)
        {
            Articles = filteredArticleLists[BaseArticleKey];
            NewArticles = filteredArticleLists[NewArticleKey];
        }
    }
}
