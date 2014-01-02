using System.Windows;
using EbaySeller.Pages.Import;
using EbaySeller.ViewModel.Source.ViewInterfaces;
using GalaSoft.MvvmLight.Ioc;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            log4net.Config.XmlConfigurator.Configure();
            SimpleIoc.Default.Register<IImportListPage>(()=> new ImportListPage());
            SimpleIoc.Default.Register<IWheelDetailListPage>(()=> new WheelDetailListPage());
        }
    }
}
