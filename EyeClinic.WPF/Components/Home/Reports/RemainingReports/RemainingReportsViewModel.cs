using EyeClinic.WPF.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EyeClinic.WPF.Components.Home.Reports.RemainingReports
{
    public partial class RemainingReportsViewModel
    {
        public RemainingReportsViewModel() { }
    }

    public partial class RemainingReportsViewModel : BaseViewModel<RemainingReportsView>
    {
        public override async Task Initialize()
        {
             await base.Initialize();
          
           FromDate = DateTime.Now; 
           ToDate = DateTime.Now;
            GetReportCommand = new RelayCommand(GetReport);
        }

        private void GetReport()
        {
           
        }

        public string ListViewContent { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public ICommand GetReportCommand { get; set; }


    }
}
