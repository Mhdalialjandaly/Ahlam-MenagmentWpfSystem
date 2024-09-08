using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Home.Reports.NamesOfPatientsWithASpecificDiseaseCondition;

namespace EyeClinic.WPF.Components.Home.Reports.NumberOfPatients
{
    public class NumberOfPatientsViewModel : BaseViewModel<NumberOfPatientsView>
    {
        private readonly IReportRepository _reportRepository;

        public NumberOfPatientsViewModel(IReportRepository reportRepository) {
            _reportRepository = reportRepository;

            FromDate = DateTime.Now.Date;
            ToDate = DateTime.Now.Date;

            GetReportCommand = new RelayCommand(GetReport);
        }


        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public List<PatientVisitDto> PatientVisits { get; set; }
        public int TotalPatients { get; set; }
        public int TotalMalePatients { get; set; }
        public int TotalFemalePatients { get; set; }
        public int TotalVisits { get; set; }
        public int TotalMaleVisits { get; set; }
        public int TotalFemaleVisits { get; set; }

        public ICommand GetReportCommand { get; set; }


        public void GetReport() {
            Task.Run(() => {
                BusyExecute(async () => {
                    var report = await _reportRepository.GetTotalVisits(FromDate.Date, ToDate.Date);
                    PatientVisits = report;

                    var pat = report.Select(e => e.Patient)
                        .Distinct().ToList();
                    var distinctPatients = new List<PatientDto>();
                    foreach (var patient in pat) {
                        if (distinctPatients.Any(e => e.Id == patient.Id))
                            continue;

                        distinctPatients.Add(patient);
                    }
                    TotalPatients = distinctPatients.Count();
                    TotalMalePatients = distinctPatients.Count(e => e.Gender);
                    TotalFemalePatients = distinctPatients.Count(e => e.Gender == false);
                    TotalVisits = PatientVisits.Count;
                    TotalMaleVisits = report.Count(e => e.Patient.Gender);
                    TotalFemaleVisits = report.Count(e => e.Patient.Gender == false);
                });
            });
        }
    }
}
