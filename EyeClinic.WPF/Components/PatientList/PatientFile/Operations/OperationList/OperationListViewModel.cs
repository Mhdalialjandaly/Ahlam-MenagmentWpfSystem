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
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Base.Extends;
using EyeClinic.WPF.Components.Dialogs.PasswordInput;
using EyeClinic.WPF.Components.PatientList.PatientFile.Operations.OperationList.DropOperationFile;
using EyeClinic.WPF.Components.PatientList.PatientFile.Operations.OperationList.OperationEditor;
using EyeClinic.WPF.Components.PatientList.PatientFile.Operations.OperationList.OperationHistory;
using EyeClinic.WPF.Setup;
using Unity;

namespace EyeClinic.WPF.Components.PatientList.PatientFile.Operations.OperationList
{
    public partial class OperationListViewModel
    {
        public OperationListViewModel() {
            if (IsDesignMode) {
                Operations = new ObservableCollection<PatientOperationDto> {
                    new() {MedicalCenter = new() {EnName = "Test"}, OperationDate = DateTime.Now}
                };
            }
        }
    }

    public partial class OperationListViewModel : BaseViewModel<OperationListView>
    {
        private readonly IPatientOperationRepository _patientOperationRepository;
        private readonly IUnityContainer _container;
        private readonly IDialogService _dialogService;

        public OperationListViewModel(IPatientOperationRepository patientOperationRepository,
            IUnityContainer container, IDialogService dialogService) {
            _patientOperationRepository = patientOperationRepository;
            _container = container;
            _dialogService = dialogService;

            AddOperationCommand = new RelayCommand(AddOperation);
            EditOperationCommand = new RelayCommand<PatientOperationDto>(EditOperation);
            DeleteOperationCommand = new RelayCommand<PatientOperationDto>(DeleteOperation);
            FinishOperationCommand = new RelayCommand(FinishOperation);
            OperationHistoryCommand = new RelayCommand(OperationHistory);
        }


        public override Task Initialize() {
            Operations = new ObservableCollection<PatientOperationDto>();
            return base.Initialize();
        }

        public async Task Initialize(int patientId) {
            PatientId = patientId;

            Operations = new ObservableCollection<PatientOperationDto>(
                await _patientOperationRepository
                    .GetByKey(e => e.PatientId == patientId));
        }

        public event EventHandler<PatientOperationDto> OnSelectedPatientOperationChanged;
        public Action OnSave;

        public int PatientId { get; set; }
        public bool OperationCompleted { get; set; }

        public ObservableCollection<PatientOperationDto> Operations { get; set; }

        private PatientOperationDto _selectedOperation;
        public PatientOperationDto SelectedOperation {
            get => _selectedOperation;
            set {
                _selectedOperation = value;
                OperationCompleted = value?.IsFinish ?? false;
                OnSelectedPatientOperationChanged?.Invoke(this, value);
            }
        }



        public ICommand AddOperationCommand { get; set; }
        public ICommand EditOperationCommand { get; set; }
        public ICommand DeleteOperationCommand { get; set; }
        public ICommand FinishOperationCommand { get; set; }
        public ICommand OperationHistoryCommand { get; set; }

        public ICommand CancelCommand => new RelayCommand(() => {
            ContainerHandler.Container.Resolve<IDialogService>().DisposeLastDialog();
        });
        public PatientFileViewModel PatientFileViewModel { get; set; }


        public void AddOperation() {
            BusyExecute(async () => {
                var editor = _container.Resolve<OperationEditorViewModel>();
                await editor.Initialize(PatientId);
                editor.Operation = Core.Enums.Operation.Add;

                _dialogService.ShowEditorDialog(editor.GetView() as OperationEditorView,
                    async () => {
                        var patientOperation = await editor.SaveAsync();
                        if (patientOperation == null)
                            return false;

                        Operations.Insert(0, patientOperation);
                        OnSave?.Invoke();
                        return true;
                    });
            });
        }

        public void EditOperation(PatientOperationDto operation) {
            if (!_container.CheckUserRole(UserRoles.Admin))
                return;           
          
            if (operation == null)
                return;
            SelectedOperation = operation;

            BusyExecute(async () => {
                var editor = _container.Resolve<OperationEditorViewModel>();
                await editor.Initialize(PatientId);
                editor.Operation = Core.Enums.Operation.Edit;
                editor.Id = SelectedOperation.Id;
                editor.BuildFromModel(SelectedOperation);

                _dialogService.ShowEditorDialog(editor.GetView() as OperationEditorView,
                    async () => {
                        var patientOperation = await editor.SaveAsync();
                        if (patientOperation == null)
                            return false;

                        await Initialize(PatientId);
                        return true;
                    });
            });
        }

        public void DeleteOperation(PatientOperationDto patientOperation) {
            if (MessageBox.Show("Do You Want To Delete ??", "Conform", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (patientOperation == null)
                return;
            BusyExecute(async () => {
                await _patientOperationRepository.Delete(patientOperation.Id);
                await _container.Resolve<IPatientOperationSessionRepository>()
                    .Delete(e => e.PatientOperationId == patientOperation.Id);
                Operations.Remove(patientOperation);
            });
            }
            else
            {
                return;
            }
        }

        public void FinishOperation() {
            if (SelectedOperation == null)
                return;

            var keyword = SelectedOperation.IsFinish ? "Resume" : "Complete";
            _dialogService.ShowConfirmationMessage($"Are you sure you want to {keyword} Operation",
                async () => {
                    var isOperationFinished = !SelectedOperation.IsFinish;

                    await UpdateOperationStatus(SelectedOperation.Id, isOperationFinished);
                    OperationCompleted = isOperationFinished;

                    //if (isOperationFinished) {
                    //    var dropEyeTests = _container.Resolve<DropOperationFileViewModel>();
                    //    await dropEyeTests.Initialize(PatientId);

                    //    _dialogService.ShowInformationDialog(
                    //        dropEyeTests.GetView() as DropOperationFileView, () => {
                    //            BusyExecute(async () => {
                    //                await dropEyeTests.DropSelectedItems();

                    //                await Initialize(PatientId);
                    //                _dialogService.DisposeLastDialog();
                    //            });
                    //        });
                    //    return false;
                    //}

                    await Initialize(PatientId);
                    return true;
                });
        }

        private void OperationHistory() {
            var operationHistory = _container.Resolve<OperationHistoryViewModel>();
            operationHistory.InitializeList(Operations.ToList());

            _dialogService.ShowInformationDialog(operationHistory.GetView() as OperationHistoryView);
        }

        public async Task UpdateOperationStatus(int patientOperationId, bool finish) {
            await _patientOperationRepository.UpdateOperationStatus(patientOperationId, finish);
        }
    }
}
