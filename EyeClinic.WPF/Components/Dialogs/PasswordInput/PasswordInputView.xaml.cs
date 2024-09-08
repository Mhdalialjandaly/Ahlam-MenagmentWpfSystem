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


namespace EyeClinic.WPF.Components.Dialogs.PasswordInput
{
    /// <summary>
    /// Interaction logic for PasswordInputView.xaml
    /// </summary>
    public partial class PasswordInputView : UserControl
    {
        public PasswordInputView() {
            InitializeComponent();
            
        }

        private void PasswordBoxControl_Loaded(object sender, RoutedEventArgs e)
        {
            PasswordBoxControl.Focus();
        }
    }
}
