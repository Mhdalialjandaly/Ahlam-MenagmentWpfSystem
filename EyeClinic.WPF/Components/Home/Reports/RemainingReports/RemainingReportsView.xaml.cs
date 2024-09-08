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
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System.Data;
using Connecting;

namespace EyeClinic.WPF.Components.Home.Reports.RemainingReports
{
    /// <summary>
    /// Interaction logic for RemainingReportsView.xaml
    /// </summary>
    public partial class RemainingReportsView : Window
    {
        DBConnect dBCon = new DBConnect();
        public RemainingReportsView()
        {
            InitializeComponent();
            //FromDate.DisplayDate = DateTime.Today.Date;
        }
       
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    string selectQuerry = "select *  from PatientVisits where CreatedDate >= 22/5/2023";
            //    SqlCommand command = new SqlCommand(selectQuerry, dBCon.GetCon());
            //    SqlDataAdapter adapter = new SqlDataAdapter(command);
            //    DataTable table = new DataTable();
            //    adapter.Fill(table);
            //    ListView.DataContext = table;
            //    AccountNumber.Text = table.Rows[0].Field<int>("Id").ToString();
            //}
            //catch (Exception eex)
            //{
            //    MessageBox.Show(eex.Message);
            //}
        }
    }
}
