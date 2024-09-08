using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.DataAccess.Repositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Base.Extends;
using EyeClinic.WPF.Components.PatientList.PatientFile.EyeDiagnosis.EyeDiagnosisEditor;
using EyeClinic.WPF.Components.PatientList.PatientFile.EyeDiagnosis.EyeDiagnosisHistory;
using Microsoft.EntityFrameworkCore;
using Unity;

namespace EyeClinic.WPF.Components.PatientList.PatientFile.EyeDiagnosis
{
    public partial class EyeDiagnosisViewModel
    {
        public EyeDiagnosisViewModel() { }
    }

    public partial class EyeDiagnosisViewModel : BaseViewModel<EyeDiagnosisView>
    {
        private readonly IDiagnosisRepository _diagnosisRepository;
        private readonly IPatientVisitDiagnosisRepository _patientVisitDiagnosisRepository;
        private readonly IPatientVisitRepository _patientVisitRepository;
        private readonly IDialogService _dialogService;
        private readonly IUnityContainer _container;

        public EyeDiagnosisViewModel(IDiagnosisRepository diagnosisRepository,
            IPatientVisitDiagnosisRepository patientVisitDiagnosisRepository,
            IPatientVisitRepository patientVisitRepository,
            IDialogService dialogService, IUnityContainer container) {
            _diagnosisRepository = diagnosisRepository;
            _patientVisitDiagnosisRepository = patientVisitDiagnosisRepository;
            _patientVisitRepository = patientVisitRepository;
            _dialogService = dialogService;
            _container = container;

            AddLeftEyeDiagnosisCommand = new RelayCommand(AddLeftEyeDiagnosis);
            AddRightEyeDiagnosisCommand = new RelayCommand(AddRightEyeDiagnosis);
            DeleteEyeDiagnosisCommand = new RelayCommand<PatientVisitDiagnosis>(DeleteEyeDiagnosis);
            AddDiagnosisCommand = new RelayCommand(AddDiagnosis);
            DiagnosisHistoryCommand = new RelayCommand(DiagnosisHistory);
        }

        public override Task Initialize() {
            throw new Exception("User Initialize with param");
        }

        public async Task Initialize(int patientVisitId) {
            PatientVisitId = patientVisitId;

            BaseDiagnosisList = await _diagnosisRepository.GetAll();
            DiagnosisList = new ObservableCollection<DiagnosisDto>(BaseDiagnosisList);

            var requiredDiagnoses = await _patientVisitDiagnosisRepository
                .GetByKey(e => e.PatientVisitId == patientVisitId);
            if (requiredDiagnoses == null)
                return;

            VisitDate = await _patientVisitRepository
               .GetVisitDateById(PatientVisitId);

            LeftDiagnoses = new ObservableCollection<PatientVisitDiagnosis>(
                requiredDiagnoses.Where(e => e.LeftEye));
            RightDiagnoses = new ObservableCollection<PatientVisitDiagnosis>(
                requiredDiagnoses.Where(e => e.LeftEye == false));

            var item = await _patientVisitRepository.Get().AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == patientVisitId);

            if (item != null)
                PatientId = item.PatientId;
        }
        public DateTime? VisitDate { get; set; }
        public int PatientVisitId { get; set; }
        public int PatientId { get; set; }
        public ObservableCollection<DiagnosisDto> DiagnosisList { get; set; }
        public List<DiagnosisDto> BaseDiagnosisList { get; set; }
        public DiagnosisDto SelectedDiagnosis { get; set; }

        public ObservableCollection<PatientVisitDiagnosis> LeftDiagnoses { get; set; }
        public ObservableCollection<PatientVisitDiagnosis> RightDiagnoses { get; set; }

        private string _searchText;
        public string SearchText {
            get => _searchText;
            set {
                _searchText = value;
                SearchDiagnosis(value);
            }
        }


        public ICommand AddLeftEyeDiagnosisCommand { get; set; }
        public ICommand AddRightEyeDiagnosisCommand { get; set; }
        public ICommand DeleteEyeDiagnosisCommand { get; set; }
        public ICommand AddDiagnosisCommand { get; set; }
        public ICommand DiagnosisHistoryCommand { get; set; }

        public ICommand CancelCommand => new RelayCommand(() => {
            ContainerHandler.Container.Resolve<IDialogService>().DisposeLastDialog();
        });
        public PatientFileViewModel PatientFileViewModel { get; set; }

        public void SearchDiagnosis(string key) {
            DiagnosisList = new ObservableCollection<DiagnosisDto>(
                BaseDiagnosisList
                    .Where(e => e.ArName.ToLower().StartsWith(key.ToLower()) ||
                                e.EnName.ToLower().StartsWith(key.ToLower())));
        }

        public Action OnSave;
        public void AddLeftEyeDiagnosis() {
            if (SelectedDiagnosis == null)
                return;

            if (LeftDiagnoses.Any(e =>
                e.Diagnosis.Code == SelectedDiagnosis.Code && e.LeftEye))
                return;

            BusyExecute(async () => {
                var item = await _patientVisitDiagnosisRepository.Add(new PatientVisitDiagnosis {
                    DiagnosisId = SelectedDiagnosis.Id,
                    PatientVisitId = PatientVisitId,
                    LeftEye = true
                });

                item.Diagnosis = SelectedDiagnosis;
                LeftDiagnoses.Add(item);
                OnSave?.Invoke();
            });
        }

        public void AddRightEyeDiagnosis() {
            if (SelectedDiagnosis == null)
                return;

            if (RightDiagnoses.Any(e =>
                e.Diagnosis.Code == SelectedDiagnosis.Code && e.LeftEye == false))
                return;

            BusyExecute(async () => {
                var item = await _patientVisitDiagnosisRepository.Add(new PatientVisitDiagnosis {
                    DiagnosisId = SelectedDiagnosis.Id,
                    PatientVisitId = PatientVisitId,
                    LeftEye = false
                });

                item.Diagnosis = SelectedDiagnosis;
                RightDiagnoses.Add(item);
                OnSave?.Invoke();
            });
        }

        public void DeleteEyeDiagnosis(PatientVisitDiagnosis visitDiagnosis) {
           
                BusyExecute(async () =>
                {
                    await _patientVisitDiagnosisRepository
                        .Delete(visitDiagnosis.Id);

                    if (visitDiagnosis.LeftEye)
                    {
                        LeftDiagnoses.Remove(visitDiagnosis);
                        return;
                    }

                    RightDiagnoses.Remove(visitDiagnosis);
                    OnSave?.Invoke();
                });
        
        }

        private void AddDiagnosis() {
            var diagnosisEditor = _container.Resolve<EyeDiagnosisEditorViewModel>();
            _dialogService.ShowEditorDialog(diagnosisEditor.GetView() as EyeDiagnosisEditorView,
                async () => {
                    var item = await diagnosisEditor.Save();
                    if (item == null)
                        return false;

                    DiagnosisList.Add(item);
                    return true;
                });
        }

        private void DiagnosisHistory() {
            BusyExecute(async () => {
                var history = _container.Resolve<EyeDiagnosisHistoryViewModel>();
                await history.Initialize(PatientId);

                _dialogService.ShowInformationDialog(history.GetView() as EyeDiagnosisHistoryView);
            });
        }
    }
}
