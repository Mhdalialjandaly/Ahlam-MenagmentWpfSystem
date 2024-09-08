using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.Localization;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using Microsoft.EntityFrameworkCore;
using Unity;

namespace EyeClinic.WPF.Components.Dialogs.AskForCost
{
    public partial class AskForCostViewModel
    {
        public AskForCostViewModel() {
            if (IsDesignMode) { }
        }
    }

    public partial class AskForCostViewModel : BaseViewModel<AskForCostView>
    {
        private readonly IUnityContainer _container;
        private readonly IPatientVisitRepository _patientVisitRepository;
        private readonly INotPayReasonRepository _notPayReasonRepository;
        private readonly IVisitTypeRepository _visitTypeRepository;
        private readonly INotificationService _notificationService;

        public AskForCostViewModel(IUnityContainer container, IPatientVisitRepository patientVisitRepository,
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

        public async Task Initialize(PatientDto patient) {
            GetView();

            VisitDate = DateTime.Now.Date;
            NotPayReasons = await _notPayReasonRepository.GetAll();
            VisitTypes = await _visitTypeRepository.GetAll();
        }

        public int Id { get; set; }
        public int Cost { get; set; }
        public DateTime VisitDate { get; set; }
        public int Payment { get; set; }
        public string Note { get; set; }
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
                Cost = SelectedVisitType.Cost + (IncludeRetinalImageService ? 75000 : 0);
            }
        }


        private bool _includeRetinalImageService;

        public bool IncludeRetinalImageService {
            get => _includeRetinalImageService;
            set {
                _includeRetinalImageService = value;
                if (value)
                    Cost += 30000;
                else
                    Cost -= 30000;
            }
        }


        public async Task<PatientVisitDto> Save(int patientId) {
            try {
                if (!await IsValidForSave(patientId))
                    return null;

                var patientVisit = BuildFromProperties(patientId);

                var visit = await _patientVisitRepository.Add(patientVisit);
                visit.NotPaymentReason = SelectedNotPayReason ;
                return visit;
            } catch (Exception ex) {
                _container.Resolve<INotificationService>()
                    .Error("This Oreder already in the Queue");
                LogError(ex.Message, ex);
                return null;
            }
        }

        private PatientVisitDto BuildFromProperties(int patientId) {
            var visitDate = VisitDate != DateTime.MinValue ? VisitDate : DateTime.Now;
            return new() {
                
                Cost = Cost,
                PatientId = patientId,
                VisitDate = visitDate,
                Notes = Note,
                Payment = Payment,
                PatientVisitType = SelectedVisitType.EnName,
                CreatedDate = CreatedDate,
                NotPaymentReasonId = SelectedNotPayReason?.Id,
            };
        }

        private async Task<bool> IsValidForSave(int patientId) {
            var exist = await _patientVisitRepository.Get().AsNoTracking()
                .FirstOrDefaultAsync(e => e.PatientId == patientId &&
                                          VisitDate == e.VisitDate &&
                                          e.DeletedDate == null);
            if (exist != null) {
                _notificationService.Warning(
                    _container.Resolve<ILocalizationService>()
                            .Localize("OrderAlreadyExistInQueue"));
                return false;
            }

            if (SelectedVisitType == null) {
                _notificationService.Warning(
                    _container.Resolve<ILocalizationService>()
                        .Localize("InvalidOrderType"));
                return false;
            }

            if (Payment < 0) {
                _notificationService.Warning(
                    _container.Resolve<ILocalizationService>()
                        .Localize("PaymentMustBeBiggerThanZero"));
                return false;
            }
            if (Payment > Cost) {
                _notificationService.Warning(
                    _container.Resolve<ILocalizationService>()
                        .Localize("PaymentMustBeLessOrEqualToCost"));
                return false;
            }

            if (Payment < Cost && SelectedNotPayReason == null) {
                _notificationService.Warning(
                    _container.Resolve<ILocalizationService>()
                        .Localize("YouHaveToChooseTheReasonOfNotPayment"));
                return false;
            }

            return true;
        }
    }
}
