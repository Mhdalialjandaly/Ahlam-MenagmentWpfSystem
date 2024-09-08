using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using EyeClinic.Core;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.PatientList.PatientFile.Operations.OperationList.OperationSessionList.OperationSessionEditor;
using Unity;
using Operation = EyeClinic.Core.Enums.Operation;

namespace EyeClinic.WPF.Components.PatientList.PatientFile.Operations.OperationList.OperationSessionList
{
    public class OperationSessionListViewModel : BaseViewModel<OperationSessionListView>
    {
        private readonly IPatientOperationSessionRepository _operationSessionRepository;
        private readonly IUnityContainer _container;
        private readonly IDialogService _dialogService;

        public OperationSessionListViewModel(IPatientOperationSessionRepository operationSessionRepository,
            IUnityContainer container, IDialogService dialogService) {
            _operationSessionRepository = operationSessionRepository;
            _container = container;
            _dialogService = dialogService;

            AddSessionCommand = new RelayCommand(AddSession);
            EditSessionCommand = new RelayCommand(EditSession);
            DeleteSessionCommand = new RelayCommand<PatientOperationSessionDto>(DeleteSession);
        }


        public override Task Initialize() {
            Sessions = new ObservableCollection<PatientOperationSessionDto>();
            return base.Initialize();
        }

        public async Task Initialize(int patientOperationId) {
            PatientOperationId = patientOperationId;

            Sessions = new ObservableCollection<PatientOperationSessionDto>(
                (await _operationSessionRepository
                    .GetByKey(e => e.PatientOperationId == patientOperationId))
                .OrderByDescending(e => e.SessionDate));
        }

        public event EventHandler<PatientOperationSessionHandler> OnOperationSessionAdded;
        public event EventHandler<PatientOperationSessionValueDifferent> OnOperationSessionEditedDiffValue;
        public int PatientOperationId { get; set; }

        public ObservableCollection<PatientOperationSessionDto> Sessions { get; set; }
        public PatientOperationSessionDto SelectedSession { get; set; }


        public ICommand AddSessionCommand { get; set; }
        public ICommand EditSessionCommand { get; set; }
        public ICommand DeleteSessionCommand { get; set; }


        public void AddSession() {
            if (PatientOperationId == 0) {
                _container.Resolve<INotificationService>().Warning("Choose Operation First");
                return;
            }

            BusyExecute(async () => {
                var editor = _container.Resolve<OperationSessionEditorViewModel>();
                editor.PatientOperationId = PatientOperationId;
                await editor.Initialize();
                editor.Operation = Operation.Add;

                _dialogService.ShowEditorDialog(editor.GetView() as OperationSessionEditorView,
                    async () => {
                        var operationSession = await editor.SaveAsync();
                        if (operationSession == null)
                            return false;

                        OnOperationSessionAdded?.Invoke(this, new PatientOperationSessionHandler {
                            FinishOperation = editor.FinishOperation,
                            PatientOperationSession = operationSession
                        });
                        Sessions.Insert(0, operationSession);
                        return true;
                    });
            });
        }

        public void EditSession() {
            if (SelectedSession == null)
                return;

            BusyExecute(async () => {
                var editor = _container.Resolve<OperationSessionEditorViewModel>();
                await editor.Initialize();
                editor.Operation = Operation.Edit;
                editor.PatientOperationId = PatientOperationId;
                editor.BuildFromModel(SelectedSession);
                var basePayment = SelectedSession.PaymentValue;

                _dialogService.ShowEditorDialog(editor.GetView() as OperationSessionEditorView,
                    async () => {
                        var operationSession = await editor.SaveAsync();
                        if (operationSession == null)
                            return false;

                        var value = operationSession.PaymentValue - basePayment;
                        OnOperationSessionEditedDiffValue?.Invoke(this,
                            new PatientOperationSessionValueDifferent(operationSession.PatientOperationId, value, editor.FinishOperation));
                        Sessions.UpdateItem(SelectedSession, operationSession);
                        return true;
                    });
            });
        }

        public void DeleteSession(PatientOperationSessionDto operationSession) {
            if (operationSession == null)
                return;

            BusyExecute(async () => {
                await _operationSessionRepository.Delete(operationSession.Id);
                Sessions.Remove(operationSession);
            });
        }
    }

    public class PatientOperationSessionHandler
    {
        public PatientOperationSessionDto PatientOperationSession { get; set; }
        public bool FinishOperation { get; set; }
    }

    public class PatientOperationSessionValueDifferent
    {
        public PatientOperationSessionValueDifferent(int patientOperationId, int diffValue, bool finishOperation) {
            PatientOperationId = patientOperationId;
            DiffValue = diffValue;
            FinishOperation = finishOperation;
        }

        public int PatientOperationId { get; set; }
        public int DiffValue { get; set; }
        public bool FinishOperation { get; set; }
    }
}
