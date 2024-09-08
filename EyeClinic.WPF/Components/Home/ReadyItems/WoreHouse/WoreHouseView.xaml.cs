using EyeClinic.WPF.Components.Home.ReadyItems.WoreHouse.WareHouseEditer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EyeClinic.WPF.Components.Home.ReadyItems.WoreHouse
{
    /// <summary>
    /// Interaction logic for WoreHouseView.xaml
    /// </summary>
    public partial class WoreHouseView : UserControl
    {
        public WoreHouseView()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((WoreHouseViewModel)this.DataContext).SearchCommand.Execute(null);
        }

        
    }
}
