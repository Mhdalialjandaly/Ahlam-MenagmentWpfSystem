using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.Base;

namespace EyeClinic.WPF.Components.Home.Reports.PatientByMedicineReport
{
    public class PatientByMedicineReportViewModel : BaseViewModel<PatientByMedicineReportView>
    {
        private readonly IOldMedicineRepository _oldMedicineRepository;
        private readonly IMedicineRepository _medicineRepository;

        public PatientByMedicineReportViewModel(IOldMedicineRepository oldMedicineRepository,
            IMedicineRepository medicineRepository) {
            _oldMedicineRepository = oldMedicineRepository;
            _medicineRepository = medicineRepository;
            FromDate = DateTime.Now.Date;
            ToDate = DateTime.Now.Date;

            GetReportCommand = new RelayCommand(GetReport);
        }

        public override async Task Initialize() {
            Medicines = await _medicineRepository.GetAll();
        }

        public int TotalPatients { get; set; }

        public List<MedicineDto> Medicines { get; set; }
        public MedicineDto SelectedMedicine { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public ICommand GetReportCommand { get; set; }

        public List<PatientMedicineReportModel> ReportData { get; set; }


        public void GetReport() {
            if (SelectedMedicine == null)
                return;

            Task.Run(() => {
                BusyExecute(async () => {
                    var report = await _oldMedicineRepository
                        .GetByDateAndMedicine(FromDate.Date, ToDate.Date, SelectedMedicine.MedicineName);

                    var patients = new List<int>();
                    foreach (var reportItem in report) {
                        if (!patients.Contains(reportItem.PatientVisit.PatientId)) {
                            patients.Add(reportItem.PatientVisit.PatientId);
                        }
                    }

                    TotalPatients = patients.Distinct().Count();

                    ReportData = report.Select(e => new PatientMedicineReportModel() {
                        VisitDate = e.PatientVisit.VisitDate,
                        PatientName = e.PatientVisit.Patient.FullName,
                    }).ToList();
                });
            });
        }
    }

    public class PatientMedicineReportModel
    {
        public string PatientName { get; set; }
        public DateTime VisitDate { get; set; }
    }
}
