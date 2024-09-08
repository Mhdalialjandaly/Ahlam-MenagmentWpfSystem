using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Documents;
using EyeClinic.Core;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.PatientList.PatientFile.Operations.OperationList;
using EyeClinic.WPF.Components.PatientList.PatientFile.Operations.OperationList.OperationSessionList;
using Unity;

namespace EyeClinic.WPF.Components.PatientList.PatientFile.Operations
{
    public partial class OperationsViewModel
    {
        public OperationsViewModel() {
            if (IsDesignMode) {
                SelectedPatientOperationRemaining = 948710581;
            }
        }
    }

    public partial class OperationsViewModel : BaseViewModel<OperationsView>
    {
        private readonly IUnityContainer _container;
        private readonly IDialogService _dialogService;

        public OperationsViewModel(IUnityContainer container, IDialogService dialogService) {
            _container = container;
            _dialogService = dialogService;
        }

        public Action OnSave;

        public override Task Initialize() {
            throw new Exception("Use overloaded initialize method with visitId");
        }

        public async Task Initialize(int patientId) {
            PatientId = patientId;

            var operation = _container.Resolve<OperationListViewModel>();
            await operation.Initialize(patientId);
            operation.OnSave += () => {
                OnSave?.Invoke();
            };
            operation.PatientFileViewModel = PatientFileViewModel;
            operation.OnSelectedPatientOperationChanged += OnPatientOperationSelectedChanged;
            PatientOperationList = operation.GetView() as OperationListView;

            var sessions = _container.Resolve<OperationSessionListViewModel>();
            await sessions.Initialize();
            sessions.OnOperationSessionAdded +=
                async (_, session) => {
                    SelectedPatientOperationRemaining -= session.PatientOperationSession.PaymentValue;

                    await operation.UpdateOperationStatus(
                        session.PatientOperationSession.PatientOperationId, session.FinishOperation);
                    await operation.Initialize(PatientId);
                };

            sessions.OnOperationSessionEditedDiffValue += async (_, session) => {
                SelectedPatientOperationRemaining -= session.DiffValue;

                await operation.UpdateOperationStatus(
                    session.PatientOperationId, session.FinishOperation);
                await operation.Initialize(PatientId);
            };

            OperationSessionList = sessions.GetView() as OperationSessionListView;
        }

        private async void OnPatientOperationSelectedChanged(object sender, PatientOperationDto e) {
            SelectedPatientOperation = e;
            SelectedPatientOperationRemaining = e?.Remaining;
            try
            {
                if (!SelectedPatientOperation.MedicalCenterReserved && !SelectedPatientOperation.IsFinish==true)
                {
                    SelectedPatientOperationRemaining = 0;
                }
            }
            catch (Exception)
            {

                return;
            }
          
            
            ShowOperationSession = false;

            if (e == null) {
                ShowOperationSession = false;
                return;
            }

            var operationCodes = new List<string>();

            if (e.LeftEyeOperationId.HasValue && e.LeftEyeOperation != null)
                operationCodes.Add(e.LeftEyeOperation.Code);
            if (e.RightEyeOperationId.HasValue && e.RightEyeOperation != null)
                operationCodes.Add(e.RightEyeOperation.Code);

            if (operationCodes.IsNullOrEmpty()) {
                ShowOperationSession = false;
                return;
            }

            foreach (var operationCode in operationCodes) {
                if (Global.OperationWithSessionCodes.Any(c => c == operationCode)) {
                    ShowOperationSession = true;
                    break;
                }
            }

            await ((OperationSessionListViewModel)
                    OperationSessionList.DataContext)
                        .Initialize(e.Id);
        }

        public int PatientId { get; set; }
        public PatientOperationDto SelectedPatientOperation { get; set; }
        public int? SelectedPatientOperationRemaining { get; set; }

        public OperationListView PatientOperationList { get; set; }
        public OperationSessionListView OperationSessionList { get; set; }
        public bool ShowOperationSession { get; set; }
        public PatientFileViewModel PatientFileViewModel { get; set; }
    }
}
