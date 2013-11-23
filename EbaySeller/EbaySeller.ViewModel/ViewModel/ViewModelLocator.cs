/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:EbaySeller.ViewModel"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using EbaySeller.ViewModel.Source.Import;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace EbaySeller.ViewModel.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<ImportViewModel>();
            SimpleIoc.Default.Register<WheelDetailListViewModel>();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
        
        public ImportViewModel Import
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ImportViewModel>();
            }
        }
        public WheelDetailListViewModel Wheel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<WheelDetailListViewModel>();
            }
        }
        
        public static void Cleanup()
        {
            SimpleIoc.Default.Unregister<MainViewModel>();
            SimpleIoc.Default.Unregister<ImportViewModel>();
            SimpleIoc.Default.Unregister<WheelDetailListViewModel>();
        }
    }
}