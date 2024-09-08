using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using EyeClinic.Core;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.Model.Custom;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.AppServices.Localization;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.PaymentTypeList.PaymentList.PaymentEditor;
using Unity;
using Operation = EyeClinic.Core.Enums.Operation;

namespace EyeClinic.WPF.Components.PaymentTypeList.PaymentList
{
    public partial class PaymentListViewModel
    {
        public PaymentListViewModel() {
            if (IsDesignMode) { }
        }
    }

    public partial class PaymentListViewModel : BaseViewModel<PaymentListView>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IDialogService _dialogService;
        private readonly IUnityContainer _container;

        public PaymentListViewModel(IPaymentRepository paymentRepository, IDialogService dialogService,
            IUnityContainer container) {
            _paymentRepository = paymentRepository;
            _dialogService = dialogService;
            _container = container;

            AddPaymentCommand = new RelayCommand(AddPayment);
            EditPaymentCommand = new RelayCommand(EditPayment);
            DeletePaymentCommand = new RelayCommand<PaymentDto>(DeletePayment);
            SetAsPaidCommand = new RelayCommand<PaymentDto>(SetAsPaid);
        }

        public async Task Initialize(int paymentTypeId) {
            PaymentTypeId = paymentTypeId;

            var payments = await _paymentRepository
                .GetByKey(e => e.PaymentTypeId == paymentTypeId);

            BasePayments = payments.OrderByDescending(e => e.PaymentDate).ToList();
            Payments = new ObservableCollection<PaymentDto>(BasePayments);
        }


        public int PaymentTypeId { get; set; }

        public event EventHandler<PaymentDto> OnSelectedPaymentChanged;
        public event EventHandler<PaymentOperation> OnPaymentChanged;

        public List<PaymentDto> BasePayments { get; set; }
        public ObservableCollection<PaymentDto> Payments { get; set; }

        private PaymentDto _selectedPayment;
        public PaymentDto SelectedPayment {
            get => _selectedPayment;
            set { _selectedPayment = value; OnSelectedPaymentChanged?.Invoke(this, value); }
        }



        public ICommand AddPaymentCommand { get; set; }
        public ICommand EditPaymentCommand { get; set; }
        public ICommand DeletePaymentCommand { get; set; }
        public ICommand SetAsPaidCommand { get; set; }

        private string _searchText;
        public string SearchText {
            get => _searchText;
            set {
                _searchText = value;
                SearchPayments(value);
            }
        }

        private void SearchPayments(string value) {
            Payments = new ObservableCollection<PaymentDto>(
                BasePayments.Where(e => e.PaymentValue.ToString().Contains(value) ||
                                        e.Notes != null && e.Notes.ToLower().Contains(value) ||
                                        e.PaymentType is { TypeName: { } } &&
                                        e.PaymentType.TypeName.ToLower().Contains(value)));
        }


        private void AddPayment() {
            if (PaymentTypeId == 0)
                return;

            BusyExecute(async () => {
                var editor = _container.Resolve<PaymentEditorViewModel>();
                await editor.Initialize();
                editor.Operation = Operation.Add;
                _dialogService.ShowEditorDialog(editor.GetView() as PaymentEditorView,
                    async () => {
                        var paymentItem = await editor.SaveAsync(PaymentTypeId);
                        if (paymentItem == null)
                            return false;

                        OnPaymentChanged?.Invoke(this, new PaymentOperation() {
                            Payment = paymentItem,
                            Operation = Operation.Add
                        });
                        return true;
                    });
            });
        }

        private void EditPayment() {
            if (SelectedPayment == null)
                return;

            BusyExecute(async () => {
                var editor = _container.Resolve<PaymentEditorViewModel>();
                await editor.Initialize();
                editor.Operation = Operation.Edit;
                editor.Id = SelectedPayment.Id;
                editor.BuildFromModel(SelectedPayment);
                _dialogService.ShowEditorDialog(editor.GetView() as PaymentEditorView,
                    async () => {
                        var paymentItem = await editor.SaveAsync(PaymentTypeId);
                        if (paymentItem == null)
                            return false;

                        OnPaymentChanged?.Invoke(this, new PaymentOperation() {
                            Payment = paymentItem,
                            Operation = Operation.Edit
                        });
                        return true;
                    });
            });
        }

        private void DeletePayment(PaymentDto payment) {
            if (payment == null)
                return;

            BusyExecute(async () => {
                await _paymentRepository.Delete(payment.Id);

                OnPaymentChanged?.Invoke(this, new PaymentOperation() {
                    Payment = payment,
                    Operation = Operation.Delete
                });
            });
        }

        private void SetAsPaid(PaymentDto payment) {
            if (payment.PaymentDate.Date > DateTime.Now.Date) {
                _container.Resolve<INotificationService>()
                    .Warning(_container.Resolve<ILocalizationService>().Localize("PaymentDateShouldBeEqualOrLessToday"));
                return;
            }

            payment.Paid = true;
            SelectedPayment = payment;
            EditPayment();
        }

        public void Clear() {
            Payments = new ObservableCollection<PaymentDto>();
        }
    }

    public class PaymentOperation
    {
        public PaymentDto Payment { get; set; }
        public Operation Operation { get; set; }
    }
}
