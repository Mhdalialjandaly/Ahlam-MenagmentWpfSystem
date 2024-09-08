using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace EyeClinic.WPF.Components.PatientList.PatientEditor
{
    /// <summary>
    /// Interaction logic for PatientEditorView.xaml
    /// </summary>
    public partial class PatientEditorView : UserControl
    {
        public PatientEditorView() {
            InitializeComponent();
        }

        private readonly Regex _regex = new("[^0-9.-]+");

        private void NumericOnlyInput(object sender, TextCompositionEventArgs e) {
            e.Handled = _regex.IsMatch(e.Text);
        }
    }
}
