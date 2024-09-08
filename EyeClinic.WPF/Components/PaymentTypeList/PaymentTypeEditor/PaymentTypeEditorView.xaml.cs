using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace EyeClinic.WPF.Components.PaymentTypeList.PaymentTypeEditor
{
    /// <summary>
    /// Interaction logic for PaymentTypeEditorView.xaml
    /// </summary>
    public partial class PaymentTypeEditorView : UserControl
    {
        public PaymentTypeEditorView() {
            InitializeComponent();
        }

        private static readonly Regex Regex = new Regex("[^0-9.-]+");

        private void NumericOnlyBehavior(object sender, TextCompositionEventArgs e) {
            if (Regex.IsMatch(e.Text))
                e.Handled = true;
        }
    }
}
