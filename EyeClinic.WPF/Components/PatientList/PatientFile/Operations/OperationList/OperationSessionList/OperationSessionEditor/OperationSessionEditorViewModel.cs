using System;
using System.Threading.Tasks;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.Base;
using Unity;
using Operation = EyeClinic.Core.Enums.Operation;

namespace EyeClinic.WPF.Components.PatientList.PatientFile.Operations.OperationList.OperationSessionList.OperationSessionEditor
{
    public partial class OperationSessionEditorViewModel
    {
        public OperationSessionEditorViewModel() {
            if (IsDesignMode) { }
        }
    }

    public partial class OperationSessionEditorViewModel : BaseViewModel<OperationSessionEditorView>
    {
        private readonly IPatientOperationSessionRepository _operationSessionRepository;
        private readonly IUnityContainer _container;

        public OperationSessionEditorViewModel(IPatientOperationSessionRepository operationSessionRepository,
                IUnityContainer container) {
            _operationSessionRepository = operationSessionRepository;
            _container = container;
            SessionDate = DateTime.Now;
        }

        public int Id { get; set; }
        public int PatientOperationId { get; set; }
        public DateTime SessionDate { get; set; }
        public int PaymentValue { get; set; }
        public string Note { get; set; }
        public DateTime CreatedDate { get; set; }
        public int SessionNumber { get; set; }

        public bool FinishOperation { get; set; }


        public async Task<PatientOperationSessionDto> SaveAsync() {
            if (!ValidForSave())
                return null;

            var operationSession = BuildFromProperties();

            if (Operation == Operation.Add) {
                var operationItem = await _operationSessionRepository
                    .Add(operationSession);

                await _container.Resolve<IOperationRepository>().CalculateRevenue(operationItem.PatientOperationId);
                return operationItem;
            }

            if (Operation == Operation.Edit && Id > 0) {
                operationSession.LastModifiedDate = DateTime.Now;
                await _operationSessionRepository.Update(operationSession);
                await _container.Resolve<IOperationRepository>().CalculateRevenue(operationSession.PatientOperationId);
                return operationSession;
            }

            return null;
        }

        private PatientOperationSessionDto BuildFromProperties() {
            return new() {
                Id = Id,
                PaymentValue = PaymentValue,
                CreatedDate = CreatedDate,
                PatientOperationId = PatientOperationId,
                SessionDate = SessionDate,
                SessionNumber = SessionNumber,
                Note = Note
            };
        }

        public void BuildFromModel(PatientOperationSessionDto selectedSession) {
            Id = selectedSession.Id;
            PatientOperationId = selectedSession.PatientOperationId;
            SessionDate = selectedSession.SessionDate;
            Note = selectedSession.Note;
            PaymentValue = selectedSession.PaymentValue;
            SessionNumber = selectedSession.SessionNumber;
            CreatedDate = selectedSession.CreatedDate;
        }

        private bool ValidForSave() {
            return PaymentValue >= 0;
        }
    }
}
