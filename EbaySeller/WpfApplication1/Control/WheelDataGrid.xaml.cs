using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EbaySeller.Common.DataInterface;

namespace EbaySeller.Control
{
    /// <summary>
    /// Interaction logic for WheelDataGrid.xaml
    /// </summary>
    public partial class WheelDataGrid : UserControl
    {
        public WheelDataGrid()
        {
            InitializeComponent();
        }

        //public static readonly DependencyProperty WheelListProperty =
        //DependencyProperty.Register("WheelList", typeof(List<object>), typeof(WheelDataGrid));

        //public List<object> WheelList
        //{
        //    get { return (List<object>) GetValue(WheelListProperty); }
        //    set { SetValue(WheelListProperty, value); }
        //}

        //public static DependencyProperty ItemsProperty =
        //    DependencyProperty.Register("WheelList", typeof(List<IWheel>), typeof(WheelDataGrid));

        //public List<IWheel> WheelList
        //{
        //    get { return (List<IWheel>)GetValue(ItemsProperty); }
        //    set { SetValue(ItemsProperty, value); }
        //}

        public readonly static DependencyProperty WheelListProperty =
            DependencyProperty.Register("WheelList",
        typeof(IEnumerable),
        typeof(WheelDataGrid),
        new PropertyMetadata(null));

        public IEnumerable WheelList
        {
            get { return GetValue(WheelListProperty) as IEnumerable; }
            set { SetValue(WheelListProperty, value); }
        }
    }
}
