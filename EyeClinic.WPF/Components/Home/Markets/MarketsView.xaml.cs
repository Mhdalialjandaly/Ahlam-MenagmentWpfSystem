using EyeClinic.WPF.Components.Home.ReadyItems;
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

namespace EyeClinic.WPF.Components.Home.Markets
{
    /// <summary>
    /// Interaction logic for MarketsView.xaml
    /// </summary>
    public partial class MarketsView : UserControl
    {
        public MarketsView()
        {
            InitializeComponent();
        }

        private void SearchOnTextChanged(object sender, TextChangedEventArgs e)
        {
            ((MarketsViewModel)this.DataContext).SearchCommand.Execute(null);
        }
    }
}
