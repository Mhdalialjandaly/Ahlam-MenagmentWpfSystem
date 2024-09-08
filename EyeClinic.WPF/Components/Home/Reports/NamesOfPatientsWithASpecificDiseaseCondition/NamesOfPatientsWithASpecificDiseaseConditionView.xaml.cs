using Microsoft.Data.SqlClient;
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
using System.Windows.Shapes;

namespace EyeClinic.WPF.Components.Home.Reports.NamesOfPatientsWithASpecificDiseaseCondition
{
    /// <summary>
    /// Interaction logic for NamesOfPatientsWithASpecificDiseaseConditionView.xaml
    /// </summary>
    public partial class NamesOfPatientsWithASpecificDiseaseConditionView : Window
    {
        public NamesOfPatientsWithASpecificDiseaseConditionView() {
            InitializeComponent();
        }

        private void GetReportCommand(object sender, RoutedEventArgs e)
        {            
            SqlCommand swq = new SqlCommand();
        }
    }
}
