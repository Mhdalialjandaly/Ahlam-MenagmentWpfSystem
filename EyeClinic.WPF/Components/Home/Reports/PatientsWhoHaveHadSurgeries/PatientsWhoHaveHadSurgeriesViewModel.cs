using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Home.Reports.NamesOfPatientsWithASpecificDiseaseCondition;
using Unity;

namespace EyeClinic.WPF.Components.Home.Reports.PatientsWhoHaveHadSurgeries
{
    public class PatientsWhoHaveHadSurgeriesViewModel : BaseViewModel<PatientsWhoHaveHadSurgeriesView>
    {
        private readonly IReportRepository _reportRepository;
        private readonly IOperationRepository _operationRepository;

        public PatientsWhoHaveHadSurgeriesViewModel(IReportRepository reportRepository,
            IOperationRepository operationRepository) {
            _reportRepository = reportRepository;
            _operationRepository = operationRepository;

            FromDate = DateTime.Now.Date;
            ToDate = DateTime.Now.Date;

            GetReportCommand = new RelayCommand(GetReport);
        }

        public override async Task Initialize() {
            Operations = await _operationRepository.GetAll();
        }


        public List<OperationDto> Operations { get; set; }
        public OperationDto SelectedOperation { get; set; }

        public List<PatientNameModel> Patients { get; set; }
        public int TotalPatients { get; set; }
        public ICommand GetReportCommand { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public void GetReport() {
            if (SelectedOperation == null)
                return;

            Task.Run(() => {
                BusyExecute(async () => {
                    var report = await _reportRepository.GetPatientNamesByOperationId(SelectedOperation.Id,FromDate,ToDate);
                    Patients = report.Select(e => new PatientNameModel {
                        Code = e.Number.ToString(),
                        FullName = e.FullName
                    }).ToList();
                    TotalPatients = Patients.Count;
                });
            });
        }
    }
}
