using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EbaySeller.Model.Source.CSV;
using EbaySeller.Model.Source.Data.Interfaces;
using EbaySeller.Model.Source.Exceptions;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace EbaySeller.ViewModel.Source.Import
{
    public class ImportViewModel: ViewModelBase
    {
        private List<IArticle> articles = new List<IArticle>();
        private Dictionary<string, IArticle> originalArticles;
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

        public RelayCommand ImportRelayCommand
        {
            get
            {
                return new RelayCommand(ImportWasClickedCommand);
            }
        }
        public RelayCommand WheelFilterToggleCommand
        {
            get
            {
                return new RelayCommand(WheelFilterWasPressed);
            }
        }

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
            get { return "Daten: " + Articles.Count; }
        }

        private void WheelFilterWasPressed()
        {
            if (WheelFilterChecked)
            {
                var resultList = new List<IArticle>();
                foreach (var article in originalArticles.Values)
                {
                    if (article is IWheel)
                    {
                        resultList.Add(article);
                    }
                }
                Articles = resultList;
                return;
            }
            Articles = new List<IArticle>(originalArticles.Values);
        }

        private void ImportWasClickedCommand()
        {
            var fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var reader = new CSVReader();
                    originalArticles = reader.ReadArticlesFromFile(fileDialog.FileName);
                    Articles = new List<IArticle>(originalArticles.Values);
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
        }
    }
}
