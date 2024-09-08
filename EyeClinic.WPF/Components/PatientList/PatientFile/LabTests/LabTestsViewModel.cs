using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using EyeClinic.Core;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.AppServices.Print;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Base.Extends;
using EyeClinic.WPF.Components.Home.Setting.LabTests.LabTestEditor;
using EyeClinic.WPF.Components.PatientList.PatientFile.LabTests.LabTestHistory;
using EyeClinic.WPF.Components.PatientList.PatientFile.LabTests.RequestLabTest;
using Microsoft.EntityFrameworkCore;
using Unity;

namespace EyeClinic.WPF.Components.PatientList.PatientFile.LabTests
{
    public partial class LabTestsViewModel
    {
        public LabTestsViewModel() {
            if (IsDesignMode) {
                PatientVisitLabTests = new ObservableCollection<PatientVisitLabTestDto>()
                {
                    new () {Done = true, LabTest = new LabTestDto() {LabTestName = "Lab Test 1"}},
                    new () {Done = true, LabTest = new LabTestDto() {LabTestName = "Lab Test 2"}},
                    new () {Done = true, LabTest = new LabTestDto() {LabTestName = "Lab Test 3"}},
                    new () {Done = true, LabTest = new LabTestDto() {LabTestName = "Lab Test 4"}},
                };
            }
        }
    }

    public partial class LabTestsViewModel : BaseViewModel<LabTestsView>
    {
        private readonly IUnityContainer _container;
        private readonly IPatientVisitRepository _patientVisitRepository;
        private readonly IPatientVisitLabTestRepository _patientVisitLabTestRepository;
        private readonly IDialogService _dialogService;

        public LabTestsViewModel(IUnityContainer container, IPatientVisitRepository patientVisitRepository,
            IPatientVisitLabTestRepository patientVisitLabTestRepository, IDialogService dialogService) {
            _container = container;
            _patientVisitRepository = patientVisitRepository;
            _patientVisitLabTestRepository = patientVisitLabTestRepository;
            _dialogService = dialogService;

            RequestLabTestCommand = new RelayCommand(RequestLabTest);
            AddNewLabTestCommand = new RelayCommand(AddNewLabTest);
            SaveRowCommand = new RelayCommand<PatientVisitLabTestDto>(SaveRow);
            DeleteRowCommand = new RelayCommand<PatientVisitLabTestDto>(DeleteRow);
            LabTestHistoryCommand = new RelayCommand(LabTestHistory);
            SaveAllCommand = new RelayCommand(SaveAll);
            PrintLabTestCommand = new RelayCommand(PrintLabTest);
        }

        public DateTime? VisitDate { get; set; }
        public int PatientVisitId { get; set; }
        public int PatientId { get; set; }

        public ObservableCollection<PatientVisitLabTestDto> PatientVisitLabTests { get; set; }

        public async Task Initialize(int patientVisitId) {
            PatientVisitId = patientVisitId;

            var item = await _patientVisitRepository.Get().AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == patientVisitId);

            VisitDate = await _patientVisitRepository
              .GetVisitDateById(PatientVisitId);

            if (item != null)
                PatientId = item.PatientId;

            var requiredLabTests = await _patientVisitLabTestRepository
                .GetByKey(e => e.PatientVisitId == PatientVisitId);

            PatientVisitLabTests = new ObservableCollection<PatientVisitLabTestDto>();

            if (!requiredLabTests.IsNullOrEmpty())
                PatientVisitLabTests = new ObservableCollection<PatientVisitLabTestDto>(requiredLabTests);
        }

        public ICommand RequestLabTestCommand { get; set; }
        public ICommand AddNewLabTestCommand { get; set; }
        public ICommand SaveRowCommand { get; set; }
        public ICommand DeleteRowCommand { get; set; }
        public ICommand LabTestHistoryCommand { get; set; }
        public ICommand SaveAllCommand { get; set; }
        public ICommand PrintLabTestCommand { get; set; }

        public ICommand CancelCommand => new RelayCommand(() => {
            ContainerHandler.Container.Resolve<IDialogService>().DisposeLastDialog();
        });
        public PatientFileViewModel PatientFileViewModel { get; set; }


        public async void RequestLabTest() {
            var request = _container.Resolve<RequestLabTestViewModel>();
            await request.Initialize();

            _dialogService.ShowEditorDialog(request.GetView() as RequestLabTestView, async () => {
                var selectedLabTests = request.LabTests
                    .Where(e => e.IsChecked).ToList();
                if (selectedLabTests.IsNullOrEmpty())
                    return false;

                await _patientVisitLabTestRepository.AddRange(
                    selectedLabTests.Select(e => new PatientVisitLabTestDto {
                        LabTestId = e.Item.Id,
                        PatientVisitId = PatientVisitId
                    }).ToList());

                await Initialize(PatientVisitId);
                OnSave?.Invoke();
                return true;
            });
        }

        public void AddNewLabTest() {
            var editor = _container.Resolve<LabTestEditorViewModel>();
            _dialogService.ShowEditorDialog(editor.GetView() as LabTestEditorView, async () => {
                var item = await editor.Save();
                return item != null;
            });
        }

        public void SaveRow(PatientVisitLabTestDto patientVisitLabTest) {
            BusyExecute(async () => {
                await _patientVisitLabTestRepository.Update(patientVisitLabTest);
                OnSave?.Invoke();
            });
        }

        public void DeleteRow(PatientVisitLabTestDto patientVisitLabTest) {
            if (MessageBox.Show("Do You Want To Delete ??", "Conform", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                BusyExecute(async () =>
                {
                    await _patientVisitLabTestRepository.Delete(patientVisitLabTest.Id);
                    PatientVisitLabTests.Remove(patientVisitLabTest);
                    OnSave?.Invoke();
                });
            }
        }

        private void LabTestHistory() {
            BusyExecute(async () => {
                var history = _container.Resolve<LabTestHistoryViewModel>();
                await history.Initialize(PatientId);

                _dialogService.ShowInformationDialog(history.GetView() as LabTestHistoryView);
            });
        }

        private void PrintLabTest() {
            BusyExecute(async () => {
                await _patientVisitLabTestRepository.UpdateRange(PatientVisitLabTests.ToList());
                OnSave?.Invoke();

                var patient = await _container.Resolve<IPatientRepository>()
                    .GetById(PatientId);
                _container.Resolve<IPrintService>()
                    .PrintLabTests(patient, PatientVisitLabTests.ToList());
            });
        }


        public Action OnSave;
        public void SaveAll() {
            BusyExecute(async () => {
                await _patientVisitLabTestRepository.UpdateRange(PatientVisitLabTests.ToList());
                OnSave?.Invoke();
            });
        }
    }
}
