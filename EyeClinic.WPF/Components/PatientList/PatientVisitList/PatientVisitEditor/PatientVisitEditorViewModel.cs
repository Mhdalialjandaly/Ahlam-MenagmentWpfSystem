using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using Unity;

namespace EyeClinic.WPF.Components.PatientList.PatientVisitList.PatientVisitEditor
{
    public partial class PatientVisitEditorViewModel
    {
        public PatientVisitEditorViewModel() {
            if (IsDesignMode) { }
            if (IsDesignMode) { }
        }
    }

    public partial class PatientVisitEditorViewModel : BaseViewModel<PatientVisitEditorView>
    {
        private readonly IUnityContainer _container;
        private readonly IPatientVisitRepository _patientVisitRepository;
        private readonly INotPayReasonRepository _notPayReasonRepository;
        private readonly IVisitTypeRepository _visitTypeRepository;
        private readonly INotificationService _notificationService;

        public PatientVisitEditorViewModel(IUnityContainer container, IPatientVisitRepository patientVisitRepository,
            INotPayReasonRepository notPayReasonRepository, IVisitTypeRepository visitTypeRepository,
                INotificationService notificationService) {
            _container = container;
            _patientVisitRepository = patientVisitRepository;
            _notPayReasonRepository = notPayReasonRepository;
            _visitTypeRepository = visitTypeRepository;
            _notificationService = notificationService;
        }

        public override Task Initialize() {
            throw new Exception("use override with param");
        }

        public async Task Initialize(PatientVisitDto patientVisit = null) {
            GetView();

            NotPayReasons = await _notPayReasonRepository.GetAll();
            VisitTypes = await _visitTypeRepository.GetAll();


            if (patientVisit == null) {
                VisitDate = DateTime.Now.Date;
                return;
            }
           
            VisitDate = patientVisit.VisitDate;
            CreatedDate = patientVisit.CreatedDate;
            Cost = patientVisit.Cost;
            Id = patientVisit.Id;
            SelectedNotPayReason = NotPayReasons
                .FirstOrDefault(e => e.Id == patientVisit.NotPaymentReasonId);
            ReviewDate = patientVisit.ReviewDate;
            Note = patientVisit.Notes;
            VisitIndex = patientVisit.VisitIndex;
            VisitStatus = patientVisit.VisitStatus;
            PatientId = patientVisit.PatientId;
            Payment = patientVisit.Payment;
            SelectedVisitType.EnName = patientVisit.PatientVisitType;
            patientVisit.PatientVisitType = SelectedVisitType.EnName;


           
        }

        public int Id { get; set; }
        public int Cost { get; set; }
        public DateTime VisitDate { get; set; }
        public int Payment { get; set; }
        public int PatientId { get; set; }
        public string Note { get; set; }
        public int VisitIndex { get; set; }
        public byte VisitStatus { get; set; }
        public DateTime? ReviewDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public List<NotPayReasonDto> NotPayReasons { get; set; }
        public NotPayReasonDto SelectedNotPayReason { get; set; }
        public List<VisitTypeDto> VisitTypes { get; set; }

        private VisitTypeDto _selectedVisitType;
        public VisitTypeDto SelectedVisitType {
            get => _selectedVisitType;
            set {
                _selectedVisitType = value;
                Cost = SelectedVisitType.Cost;
                if (IncludeRetinalImageService)
                    Cost += 30000;
            }
        }


        private bool _includeRetinalImageService;

        public bool IncludeRetinalImageService {
            get => _includeRetinalImageService;
            set {
                _includeRetinalImageService = value;
                if (value)
                    Cost += 25000;
                else
                    Cost -= 25000;
            }
        }

     

        public async Task<PatientVisitDto> Save(int patientId) {
            try {
                if (!IsValidForSave())
                    return null;

                var patientVisit = BuildFromProperties(patientId);

                if (Id == 0)
                    patientVisit = await _patientVisitRepository.Add(patientVisit);
                else
                    await _patientVisitRepository.Update(patientVisit);

                patientVisit.NotPaymentReason = SelectedNotPayReason;
                return patientVisit;
            } catch (Exception ex) {
                _container.Resolve<INotificationService>()
                    .Error("This patient already in the Queue");
                LogError(ex.Message, ex);
                return null;
            }
        }

        private PatientVisitDto BuildFromProperties(int patientId) {
            var visitDate = VisitDate != DateTime.MinValue ? VisitDate : DateTime.Now.Date;
         
            return new() {
                Id = Id,
                Cost = Cost,
                PatientId = patientId,
                VisitDate = visitDate,
                Notes = Note,
                VisitIndex = VisitIndex,
                VisitStatus = VisitStatus,
                PatientVisitType = SelectedVisitType.ToString(),
            Payment = Payment,
                CreatedDate = CreatedDate,
                NotPaymentReasonId = SelectedNotPayReason?.Id
            };
        }

        private bool IsValidForSave() {
            if (SelectedVisitType == null) {
                _notificationService.Warning("Invalid Visit Type");
                return false;
            }

            if (Payment < 0) {
                _notificationService.Warning("Payment must be bigger than zero");
                return false;
            }
            if (Payment > Cost) {
                _notificationService.Warning("Payment must be less or equal cost");
                return false;
            }

            if (Payment < Cost && SelectedNotPayReason == null) {
                _notificationService.Warning("You have to choose the reason of not payment");
                return false;
            }

            return true;
        }
    }
}
