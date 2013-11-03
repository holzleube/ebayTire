using System.Windows.Forms;
using EbaySeller.Model.Source.CSV;
using GalaSoft.MvvmLight.Command;

namespace EbaySeller.ViewModel.Source.Import
{
    public class MainViewModel : IMainViewModel
    {
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
                var reader = new CSVReader();
                reader.ReadArticlesFromFile(fileDialog.FileName);
            }
        }
    }
}
