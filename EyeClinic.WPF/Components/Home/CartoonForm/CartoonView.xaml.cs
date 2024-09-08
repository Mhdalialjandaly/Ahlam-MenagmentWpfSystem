using EyeClinic.WPF.Components.PatientList.PatientFile.Prescriptions;
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

namespace EyeClinic.WPF.Components.Home.CartoonForm
{
    /// <summary>
    /// Interaction logic for CartoonView.xaml
    /// </summary>
    public partial class CartoonView : UserControl
    {
        public CartoonView()
        {
            InitializeComponent();
        }
       

        private void SearchOnTextChanged(object sender, TextChangedEventArgs e)
        {
            ((CartoonViewModel)this.DataContext).SearchCommand.Execute(null);
        }
    }
}
