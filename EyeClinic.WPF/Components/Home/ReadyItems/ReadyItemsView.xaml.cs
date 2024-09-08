using EyeClinic.DataAccess.Entities;
using EyeClinic.Model;
using EyeClinic.WPF.Components.Home.CartoonForm;
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

namespace EyeClinic.WPF.Components.Home.ReadyItems
{
    /// <summary>
    /// Interaction logic for ReadyItemsView.xaml
    /// </summary>
    public partial class ReadyItemsView : UserControl
    {
        public ReadyItemsView()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((ReadyItemsViewModel)this.DataContext).SearchCommand.Execute(null);
        }
        private void GetItemId(object sender, SelectionChangedEventArgs e)
        {
            ((ReadyItemsViewModel)this.DataContext).GetMaterialValue();
        }
    }
}
