using System.Windows;
using EbaySeller.Pages.Import;
using EbaySeller.ViewModel.Source.ViewInterfaces;
using GalaSoft.MvvmLight.Ioc;
using log4net;
using log4net.Config;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public App()
        {
            
            SimpleIoc.Default.Register<IImportListPage>(()=> new ImportListPage());
            SimpleIoc.Default.Register<IWheelDetailListPage>(()=> new WheelDetailListPage());
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            XmlConfigurator.Configure();
            base.OnStartup(e);
            Log.Debug("test");
            Log.Error("test");
        }
    }
}
