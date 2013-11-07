using System;
using System.Collections.Generic;
using System.Windows.Forms;
using EbaySeller.Model.Source.CSV;
using EbaySeller.Model.Source.Data.Interfaces;
using EbaySeller.Model.Source.Exceptions;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace EbaySeller.ViewModel.Source.Import
{
    public class MainViewModel : ViewModelBase,IMainViewModel
    {
        private List<IArticle> articles;

        public List<IArticle> Articles
        {
            get { return articles; }
            set
            {
                articles = value;
                RaisePropertyChanged("Articles");
            }
        } 

        public RelayCommand ImportRelayCommand
        {
            get
            {
                return new RelayCommand(ImportWasClickedCommand);
            }
        }

        private void ImportWasClickedCommand()
        {
            var fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var reader = new CSVReader();
                    Articles = reader.ReadArticlesFromFile(fileDialog.FileName);
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
