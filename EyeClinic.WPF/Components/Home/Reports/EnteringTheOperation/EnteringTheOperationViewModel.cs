using EyeClinic.DataAccess.IRepositories;
using EyeClinic.WPF.Base;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EyeClinic.WPF.Components.Home.Reports.EnteringTheOperation
{
    public class EnteringTheOperationViewModel : BaseViewModel<EnteringTheOperationView>
    {
        private readonly IPatientOperationRepository _patientOperationRepository;

        public EnteringTheOperationViewModel(IPatientOperationRepository patientOperationRepository) {
            _patientOperationRepository = patientOperationRepository;

            FromDate = DateTime.Now.Date;
            ToDate = DateTime.Now.Date;

            GetReportCommand = new RelayCommand(GetReport);
        }


        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public int TotalCost { get; set; }
        public int TotalIncome { get; set; }
        public int Remaining { get; set; }

        public ICommand GetReportCommand { get; set; }

        public void GetReport() {
            Task.Run(() => {
                BusyExecute(async () => {
                    var report = await _patientOperationRepository
                        .GetByKey(e => e.OperationDate.Date >= FromDate.Date &&
                                       e.OperationDate.Date <= ToDate.Date);

                    TotalCost = report.Sum(e => e.TotalCost);
                    Remaining = report.Sum(e => e.Remaining);
                    TotalIncome = TotalCost - Remaining;
                });
            });
        }
    }
}
