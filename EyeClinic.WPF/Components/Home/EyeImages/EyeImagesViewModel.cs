using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using AutoMapper;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.PatientList;
using Microsoft.EntityFrameworkCore;
using Unity;

namespace EyeClinic.WPF.Components.Home.EyeImages
{
    public partial class EyeImagesViewModel
    {
        public EyeImagesViewModel() { }
    }

    public partial class EyeImagesViewModel : BaseViewModel<EyeImagesView>
    {
        private readonly IUnityContainer _container;
        private readonly IPatientVisitTestRepository _patientVisitTestRepository;

        public EyeImagesViewModel(IUnityContainer container,
            IPatientVisitTestRepository patientVisitTestRepository) {
            _container = container;
            _patientVisitTestRepository = patientVisitTestRepository;

            DeleteImageTestCommand = new RelayCommand<PatientVisitsTestDto>(DeleteImageTest);
        }


        public override async Task Initialize() {
            var patientList = _container.Resolve<PatientListViewModel>();
            await patientList.Initialize();
            patientList.EnableSearchByImageNumber = true;
            patientList.OnSelectPatient += OnPatientSelected;
            PatientList = patientList.GetView() as PatientListView;

            LastImageNumber = await _patientVisitTestRepository.GetLastImageNumber();
            LastScanNumber = await _patientVisitTestRepository.GetLastScanNumber();
            TotalEyeMuscles = await _patientVisitTestRepository.GetTotalEyeMuscles();
        }

        private void OnPatientSelected(object sender, PatientDto patient) {
            if (patient == null) {
                PatientVisitsTest = new ObservableCollection<PatientVisitsTestDto>();
                return;
            }

            var imagesFromSystem = new List<PatientVisitsTest>();
            var imagesFromExcelTable = new List<OldPatientEyeImage>();

            BusyExecute(async () => {
                if (patient.Id != 0) { // Get Images from the system
                    imagesFromSystem = await _patientVisitTestRepository.Get()
                        .AsNoTracking()
                        .Include(e => e.PatientVisit)
                        .Include(e => e.Test)
                        .Where(e => e.PatientVisit.PatientId == patient.Id)
                        .Where(e => !string.IsNullOrWhiteSpace(e.ImageNumber))
                        .Where(e => e.DeletedDate == null)
                        .OrderByDescending(e => e.PatientVisit.VisitDate)
                        .ToListAsync();
                } else { // Get Data from Excel Table
                    imagesFromExcelTable = await _container.Resolve<IOldPatientEyeImageRepository>()
                        .Get()
                        .AsNoTracking()
                        .Where(e => string.IsNullOrWhiteSpace(patient.FirstName) || e.FirstName.StartsWith(patient.FirstName))
                        .Where(e => string.IsNullOrWhiteSpace(patient.LastName) || e.LastName.StartsWith(patient.LastName))
                        .Where(e => e.DeletedDate == null)
                        .ToListAsync();
                }

                foreach (var oldPatientEyeImage in imagesFromExcelTable) {
                    imagesFromSystem.Add(new PatientVisitsTest {
                        PatientVisit = new PatientVisit(),
                        ImageNumber = oldPatientEyeImage.ImageNumber,
                        Test = new Test { EnName = oldPatientEyeImage.ImageType },
                        Id = oldPatientEyeImage.Id,
                        PatientVisitId = 0
                    });
                }

                if (imagesFromSystem == null)
                    return;


                // To remove any image has different number in search
                if (PatientList.DataContext is PatientListViewModel patientList) {
                    if (!string.IsNullOrWhiteSpace(patientList.ImageNumber)) {
                        foreach (var test in imagesFromSystem.ToList()) {
                            if (test.ImageNumber != patientList.ImageNumber)
                                imagesFromSystem.Remove(test);
                        }
                    }
                }


                PatientVisitsTest = new ObservableCollection<PatientVisitsTestDto>(
                    _container.Resolve<IMapper>()
                        .Map<List<PatientVisitsTestDto>>(imagesFromSystem
                            .OrderByDescending(e => e.PatientVisit.VisitDate)
                            .ToList()));
            });
        }

        public ObservableCollection<PatientVisitsTestDto> PatientVisitsTest { get; set; }
        public UserControl PatientList { get; set; }


        public ICommand DeleteImageTestCommand { get; set; }


        public int LastScanNumber { get; set; }
        public int LastImageNumber { get; set; }
        public int TotalEyeMuscles { get; set; }

        private void DeleteImageTest(PatientVisitsTestDto item) {
            BusyExecute(async () => {
                if (item.PatientVisitId == 0) // Data from excel
                    await _container.Resolve<IOldPatientEyeImageRepository>().Delete(item.Id);
                else // From the system
                    await _patientVisitTestRepository.ClearImageNumber(item.Id);
                PatientVisitsTest?.Remove(item);

                LastImageNumber = await _patientVisitTestRepository.GetLastImageNumber();
                LastScanNumber = await _patientVisitTestRepository.GetLastScanNumber();
                TotalEyeMuscles = await _patientVisitTestRepository.GetTotalEyeMuscles();
                
                _container.Resolve<INotificationService>()
                    .Success("Done !");
            });
        }
    }
}
