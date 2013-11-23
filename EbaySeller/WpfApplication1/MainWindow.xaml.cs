using System.Windows;
using System.Windows.Navigation;
using EbaySeller.Pages.Import;
using GalaSoft.MvvmLight.Ioc;

namespace EbaySeller
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.NavigationService.Navigate(new ImportListPage());
            SimpleIoc.Default.Register(() => MainFrame.NavigationService);
        }
    }
}
