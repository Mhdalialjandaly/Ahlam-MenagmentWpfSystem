using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Home.Reports.PatientVisitTestReport;

namespace EyeClinic.WPF.Components.Home.Reports.TotalPatientByAddressReport
{
    public partial class TotalPatientByAddressReportViewModel
    {
        public TotalPatientByAddressReportViewModel() { }
    }

    public partial class TotalPatientByAddressReportViewModel : BaseViewModel<TotalPatientByAddressReportView>
    {
        private readonly IPatientRepository _patientRepository;
        private readonly ILocationRepository _locationRepository;

        public TotalPatientByAddressReportViewModel(IPatientRepository patientRepository,
            ILocationRepository locationRepository) {
            _patientRepository = patientRepository;
            _locationRepository = locationRepository;

            GetReportCommand = new RelayCommand(GetReport);
        }

        public override async Task Initialize() {
            Locations = await _locationRepository.GetAll();
        }

        public List<LocationDto> Locations { get; set; }
        public LocationDto SelectedLocation { get; set; }
        public int TotalPatients { get; set; }

        public ICommand GetReportCommand { get; set; }

        public void GetReport() {
            if (SelectedLocation == null)
                return;

            Task.Run(() => {
                BusyExecute(async () => {
                    var patients = await _patientRepository
                        .GetByKey(e => e.LocationId == SelectedLocation.Id);

                    TotalPatients = patients.Count;
                });
            });
        }
    }
}
