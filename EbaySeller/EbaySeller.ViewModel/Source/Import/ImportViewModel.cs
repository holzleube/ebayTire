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
using EbaySeller.Common.Helper;
using EbaySeller.Model.Source.CSV;
using EbaySeller.Model.Source.CSV.Reader;
using EbaySeller.Model.Source.Ebay.Constants;
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

        private static readonly ILog logger = LogManager.GetLogger(typeof(ImportViewModel));

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
                return new RelayCommand(CompareEbayArticlesOnly);
            }
        }

        public RelayCommand SetArticlesToNullCommand
        {
            get
            {
                return new RelayCommand(SetArticlesToNull);
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
                logger.Debug("Exception beim Auslesen der Datei.", ex);
                MessageBox.Show("Datei kann nicht geöffnet werden, da sie bereits verwendet wird.");
            }
            catch (Exception e)
            {
                logger.Error("Unbekannter Fehler beim Öffnen der Datei"+fileName, e);
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
            foreach (var baseArticle in Articles)
            {
                try
                {
                    string key = ArticleKeyGenerator.GetKeyFromArticle(baseArticle);
                    var newArticle = newOriginalArticles[key];
                    if (!comperator.AreBothArticleEqual(baseArticle, newArticle))
                    {
                        var newArticleWithId = newArticle;
                        newArticleWithId.EbayIds = baseArticle.EbayIds;
                        resultList.Add(newArticleWithId);  
                    }
                    copyOfNewArticles.Remove(newArticle);
                }
                catch (KeyNotFoundException)
                {
                    baseArticle.Availability = 0;
                    resultList.Add(baseArticle);  
                }
            }
            resultList.AddRange(copyOfNewArticles);
            NavigateToWheelDetailListPage(resultList);
        }

        private void CompareEbayArticlesOnly()
        {
            if (Articles.Count == 0 || NewArticles.Count == 0)
            {
                MessageBox.Show("Es kann kein Update gemacht werden, da entweder die Basis oder die Vergleichsdatei fehlt.");
                return;
            }
            var ebayArticles = Articles.Where(x => x.EbayIds.Count > 0);
            var criteria = new PriceCriteria();
            var resultList = new List<IArticle>();
            foreach (var ebayArticle in ebayArticles)
            {
                var key = ArticleKeyGenerator.GetKeyFromArticle(ebayArticle);
                try
                {
                    var newArticle = newOriginalArticles[key];
                    if (!criteria.IsCriteriaSatisfied(ebayArticle, newArticle))
                    {
                        newArticle.EbayIds = ebayArticle.EbayIds;
                        if (newArticle.Availability < EbayArticleConstants.MinimumCountOfArticles)
                        {
                            newArticle.Availability = 0;
                        }
                        resultList.Add(newArticle);
                    }
                }
                catch (KeyNotFoundException)
                {
                    ebayArticle.Availability = 0;
                    resultList.Add(ebayArticle);
                }
            }
            NavigateToWheelDetailListPage(resultList);
        }

        private void SetArticlesToNull()
        {
            var resultList = new List<IArticle>();
            foreach (var articleToEdit in Articles)
            {
                articleToEdit.Availability = 0;
                resultList.Add(articleToEdit);
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
