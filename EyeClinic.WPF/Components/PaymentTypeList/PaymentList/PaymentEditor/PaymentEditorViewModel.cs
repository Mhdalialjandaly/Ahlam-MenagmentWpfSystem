using System;
using System.Threading.Tasks;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using Operation = EyeClinic.Core.Enums.Operation;

namespace EyeClinic.WPF.Components.PaymentTypeList.PaymentList.PaymentEditor
{
    public partial class PaymentEditorViewModel
    {
        public PaymentEditorViewModel() {
            if (IsDesignMode) { }
        }
    }

    public partial class PaymentEditorViewModel : BaseViewModel<PaymentEditorView>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly INotificationService _notificationService;

        public PaymentEditorViewModel(IPaymentRepository paymentRepository, INotificationService notificationService) {
            _paymentRepository = paymentRepository;
            _notificationService = notificationService;

            PaymentDate = DateTime.Now.Date;
        }

        public int Id { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime? ReminderDate { get; set; }
        public int PaymentValue { get; set; }
        public string Notes { get; set; }
        public bool Paid { get; set; }
        public DateTime CreatedDate { get; set; }


        public async Task<PaymentDto> SaveAsync(int paymentTypeId) {
            if (!ValidForSave())
                return null;

            var payment = BuildFromProperties();
            payment.PaymentTypeId = paymentTypeId;

            if (Operation == Operation.Add) {
                var paymentItem = await _paymentRepository.Add(payment);
                return paymentItem;
            }

            if (Operation == Operation.Edit && Id > 0) {
                payment.Id = Id;
                payment.CreatedDate = CreatedDate;
                payment.LastModifiedDate = DateTime.Now;
                await _paymentRepository.Update(payment);
                return payment;
            }

            return null;
        }

        private PaymentDto BuildFromProperties() {
            return new() {
                PaymentDate = PaymentDate,
                PaymentValue = PaymentValue,
                ReminderDate = ReminderDate?.Date,
                Notes = Notes,
                Paid = Paid
            };
        }

        private bool ValidForSave() {
            if (PaymentDate == DateTime.MinValue) {
                _notificationService.Warning("Payment Date is required");
                return false;
            }
            if (PaymentValue <= 0) {
                _notificationService.Warning("Cost should be bigger than zero");
                return false;
            }

            return true;
        }

        public void BuildFromModel(PaymentDto selectedPayment) {
            PaymentDate = selectedPayment.PaymentDate;
            PaymentValue = selectedPayment.PaymentValue;
            ReminderDate = selectedPayment.ReminderDate;
            Notes = selectedPayment.Notes;
            Paid = selectedPayment.Paid;
            CreatedDate = selectedPayment.CreatedDate;
        }
    }
}
