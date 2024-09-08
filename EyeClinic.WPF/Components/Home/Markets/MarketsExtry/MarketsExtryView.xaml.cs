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

namespace EyeClinic.WPF.Components.Home.Markets.MarketsExtry
{
    /// <summary>
    /// Interaction logic for MarketsExtryView.xaml
    /// </summary>
    public partial class MarketsExtryView : UserControl
    {
        public MarketsExtryView()
        {
            InitializeComponent();
        }

        private void search(object sender, TextChangedEventArgs e)
        {
            ((MarketsExtryViewModel)this.DataContext).SearchCommand.Execute(null);
        }
    }
}
