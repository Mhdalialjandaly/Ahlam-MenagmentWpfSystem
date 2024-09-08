using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.Base;

namespace EyeClinic.WPF.Components.Home.Reports.NamesOfPatientsWithASpecificDiseaseCondition
{
    public class NamesOfPatientsWithASpecificDiseaseConditionViewModel : BaseViewModel<NamesOfPatientsWithASpecificDiseaseConditionView>
    {
        private readonly IReportRepository _reportRepository;
        private readonly IDiseasesRepository _diseasesRepository;

        public NamesOfPatientsWithASpecificDiseaseConditionViewModel(
            IReportRepository reportRepository, IDiseasesRepository diseasesRepository)
        {
            _reportRepository = reportRepository;
            _diseasesRepository = diseasesRepository;

            FromDate = DateTime.Now.Date;
            ToDate = DateTime.Now.Date;

            GetReportCommand = new RelayCommand(GetReport);
        }

        public override async Task Initialize()
        {
            Diseases = await _diseasesRepository.GetAll();
        }

        public int TotalPatients { get; set; }
        public List<DiseaseDto> Diseases { get; set; }
        public DiseaseDto SelectedDisease { get; set; }
        public List<PatientNameModel> PatientNames { get; set; }

        public ICommand GetReportCommand { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public void GetReport()
        {
            if (SelectedDisease == null)
                return;

            Task.Run(() =>
            {
                BusyExecute(async () =>
                {
                    var patients = await _reportRepository
                        .GetPatientNamesByDisease(SelectedDisease, FromDate, ToDate);

                    PatientNames = patients.Select(e => new PatientNameModel()
                    {
                        Code = e.Number.ToString(),
                        FullName = e.FullName
                    }).ToList();

                    TotalPatients = PatientNames.Count;
                });
            });
        }
    }

    public class PatientNameModel
    {
        public string FullName { get; set; }
        public string Code { get; set; }
    }
}
