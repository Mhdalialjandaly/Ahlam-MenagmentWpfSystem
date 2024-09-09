using System.Windows;
using System.Windows.Controls;


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
