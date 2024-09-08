using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.WPF.Base;

namespace EyeClinic.WPF.Components.Home.Reports.TotalLensReport
{
    public partial class TotalLensReportViewModel
    {
        public TotalLensReportViewModel() { }
    }

    public partial class TotalLensReportViewModel : BaseViewModel<TotalLensReportView>
    {
        private readonly IPatientVisitGlassRepository _patientVisitGlassRepository;
        public TotalLensReportViewModel(IPatientVisitGlassRepository patientVisitGlassRepository) {
            _patientVisitGlassRepository = patientVisitGlassRepository;
            FromDate = DateTime.Now.Date;
            ToDate = DateTime.Now.Date;

            GetReportCommand = new RelayCommand(GetReport);
        }

        public int TotalLens { get; set; }
        public int TotalGlasses { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public ICommand GetReportCommand { get; set; }

        public List<PatientVisitGlassesReportModel> ReportData { get; set; }
        public void GetReport() {
            Task.Run(() => {
                BusyExecute(async () => {
                    var report = await _patientVisitGlassRepository
                        .GetByDate(FromDate.Date, ToDate.Date);

                    TotalLens = 0;
                    TotalGlasses = 0;
                    foreach (var reportItem in report) {
                        if (reportItem.AddVision)
                            TotalLens++;
                    }

                    TotalGlasses = report.Count;

                    ReportData = report.Select(e => new PatientVisitGlassesReportModel()
                    {
                        VisitDate = e.PatientVisit.VisitDate,                     
                        PatientName = e.PatientVisit.Patient.FullName
                    }).ToList();
                });

            });
        }

    }
    public class PatientVisitGlassesReportModel
    {
        public string PatientName { get; set; }
        public DateTime VisitDate { get; set; }       
    }
}
