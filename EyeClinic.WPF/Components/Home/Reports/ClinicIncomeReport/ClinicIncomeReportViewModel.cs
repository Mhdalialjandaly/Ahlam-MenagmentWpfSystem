using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.WPF.Base;

namespace EyeClinic.WPF.Components.Home.Reports.ClinicIncomeReport
{
    public class ClinicIncomeReportViewModel : BaseViewModel<ClinicIncomeReportView>
    {
        private readonly IPatientVisitRepository _patientVisitRepository;

        public ClinicIncomeReportViewModel(IPatientVisitRepository patientVisitRepository) {
            _patientVisitRepository = patientVisitRepository;

            FromDate = DateTime.Now.Date;
            ToDate = DateTime.Now.Date;

            GetReportCommand = new RelayCommand(GetReport);
        }


        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public int TotalCost { get; set; }
        public int TotalPayment { get; set; }

        public ICommand GetReportCommand { get; set; }

        public void GetReport() {
            Task.Run(() => {
                BusyExecute(async () => {
                    var report = await _patientVisitRepository
                        .GetByKey(e => e.VisitDate.Date >= FromDate.Date &&
                                       e.VisitDate.Date <= ToDate.Date);

                    TotalCost = report.Sum(e => e.Cost);
                    TotalPayment = report.Sum(e => e.Payment);
                });
            });
        }
    }
}
