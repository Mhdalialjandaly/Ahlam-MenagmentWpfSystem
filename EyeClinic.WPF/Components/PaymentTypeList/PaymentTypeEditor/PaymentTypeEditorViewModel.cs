using System;
using System.Threading.Tasks;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using Operation = EyeClinic.Core.Enums.Operation;

namespace EyeClinic.WPF.Components.PaymentTypeList.PaymentTypeEditor
{
    public partial class PaymentTypeEditorViewModel
    {
        public PaymentTypeEditorViewModel() {
            if (IsDesignMode) { }
        }
    }

    public partial class PaymentTypeEditorViewModel : BaseViewModel<PaymentTypeEditorView>
    {
        private readonly IPaymentTypeRepository _paymentTypeRepository;
        private readonly INotificationService _notificationService;

        public PaymentTypeEditorViewModel(IPaymentTypeRepository paymentTypeRepository,
                INotificationService notificationService) {
            _paymentTypeRepository = paymentTypeRepository;
            _notificationService = notificationService;
        }


        public int Id { get; set; }
        public string TypeName { get; set; }
        public string BeneficiaryName { get; set; }
        public int TotalCost { get; set; }
        public bool Debt { get; set; }
        public int TotalPayments { get; set; }
        public int Remaining { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedDate { get; set; }


        public async Task<PaymentTypeDto> SaveAsync() {
            if (ValidForSave()) {
                var payment = BuildFromProperties();
                if (Operation == Operation.Add) {
                    var paymentItem = await _paymentTypeRepository.Add(payment);
                    return paymentItem;
                }

                if (Operation == Operation.Edit && Id > 0) {
                    payment.Id = Id;
                    payment.CreatedDate = CreatedDate;
                    payment.LastModifiedDate = DateTime.Now;
                    await _paymentTypeRepository.Update(payment);
                    return payment;
                }
            }

            return null;
        }

        private PaymentTypeDto BuildFromProperties() {
            return new() {
                TypeName = TypeName,
                TotalCost = TotalCost,
                Debt = Debt,
                BeneficiaryName = BeneficiaryName,
                Note = Notes
            };
        }

        private bool ValidForSave() {
            if (string.IsNullOrWhiteSpace(TypeName)) {
                _notificationService.Warning("Payment Type is required");
                return false;
            }

            if (TotalCost <= 0) {
                _notificationService.Warning("Cost should be bigger than zero");
                return false;
            }

            return true;
        }

        public void BuildFromModel(PaymentTypeDto selectedPaymentType) {
            TypeName = selectedPaymentType.TypeName;
            BeneficiaryName = selectedPaymentType.BeneficiaryName;
            TotalCost = selectedPaymentType.TotalCost;
            Debt = selectedPaymentType.Debt;
            Remaining = selectedPaymentType.Remaining ?? 0;
            TotalPayments = selectedPaymentType.TotalPayments ?? 0;
            Notes = selectedPaymentType.Note;
            CreatedDate = selectedPaymentType.CreatedDate;
        }
    }
}
