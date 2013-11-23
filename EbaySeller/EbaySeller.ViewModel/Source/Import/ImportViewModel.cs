using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Navigation;
using EbaySeller.Model.Source.CSV;
using EbaySeller.Model.Source.Data.Interfaces;
using EbaySeller.Model.Source.Exceptions;
using EbaySeller.ViewModel.Source.Comperator;
using EbaySeller.ViewModel.Source.ViewInterfaces;
using EbaySeller.ViewModel.ViewModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using MessageBox = System.Windows.Forms.MessageBox;


namespace EbaySeller.ViewModel.Source.Import
{
    public class ImportViewModel: ViewModelBase
    {
        private List<IArticle> articles = new List<IArticle>();
        private List<IArticle> newArticles = new List<IArticle>();
        private Dictionary<string, IArticle> originalArticles = new Dictionary<string, IArticle>();
        private Dictionary<string, IArticle> newOriginalArticles = new Dictionary<string, IArticle>();
        private bool isWheelFilterChecked;

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
        #region commands

        public RelayCommand ImportRelayCommand
        {
            get
            {
                return new RelayCommand(ImportBaseDataWasClickedCommand);
            }
        }
        
        public RelayCommand ImportNewRelayCommand
        {
            get
            {
                return new RelayCommand(ImportNewBaseDataWasClickedCommand);
            }
        }
        public RelayCommand WheelFilterToggleCommand
        {
            get
            {
                return new RelayCommand(WheelFilterWasPressed);
            }
        }
        public RelayCommand CompareBaseAndNewFileCommand
        {
            get
            {
                return new RelayCommand(CompareBaseAndNewFile);
            }
        }

        #endregion

        public bool WheelFilterChecked
        {
            get { return isWheelFilterChecked; }
            set
            {
                isWheelFilterChecked = value;
                RaisePropertyChanged("WheelFilterChecked");
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

        private void WheelFilterWasPressed()
        {
            if (WheelFilterChecked)
            {
                var resultList = GetWheelFilteredList(originalArticles.Values);
                var newResultList = GetWheelFilteredList(newOriginalArticles.Values);
                Articles = resultList;
                NewArticles = newResultList;
                return;
            }
            Articles = new List<IArticle>(originalArticles.Values);
            NewArticles = new List<IArticle>(newOriginalArticles.Values);
        }

        private List<IArticle> GetWheelFilteredList(Dictionary<string, IArticle>.ValueCollection articlesToFilter)
        {
            var resultList = new List<IArticle>();
            if (articlesToFilter.Count == 0)
            {
                return resultList;
            }
            foreach (var article in articlesToFilter)
            {
                if (article is IWheel)
                {
                    resultList.Add(article);
                }
            }
            return resultList;
        }

        private void ImportBaseDataWasClickedCommand()
        {
            originalArticles = GetArticlesFromUserChosenFile();
            Articles = new List<IArticle>(originalArticles.Values);
        }
        private void ImportNewBaseDataWasClickedCommand()
        {
            newOriginalArticles = GetArticlesFromUserChosenFile();
            NewArticles = new List<IArticle>(newOriginalArticles.Values);
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
                string messageBoxText = "Es sind keine neuen Datensätze vorhanden. Wollen Sie alle Datensätze der Basis übernehmen?";
                string caption = "Leere Vergleichsdaten";
                var buttons = MessageBoxButtons.YesNo;
                var icon = MessageBoxImage.Question;
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
                    var newArticle = newOriginalArticles[originalArticle.ArticleId];
                    if (!comperator.AreBothArticleEqual(originalArticle, newArticle))
                    {
                        resultList.Add(newArticle);  
                    }
                    copyOfNewArticles.Remove(newArticle);
                }
                catch (KeyNotFoundException e)
                {
                    resultList.Add(originalArticle);  
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


        private Dictionary<string, IArticle> GetArticlesFromUserChosenFile()
        {
            var fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var reader = new CSVReader();
                    return reader.ReadArticlesFromFile(fileDialog.FileName);
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
            return new Dictionary<string, IArticle>();
        }
    }
}
