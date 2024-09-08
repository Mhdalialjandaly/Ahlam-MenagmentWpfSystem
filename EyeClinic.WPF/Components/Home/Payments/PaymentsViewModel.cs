using System.Threading.Tasks;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.PaymentTypeList;
using EyeClinic.WPF.Components.PaymentTypeList.PaymentList;
using Unity;

namespace EyeClinic.WPF.Components.Home.Payments
{
    public class PaymentsViewModel : BaseViewModel<PaymentsView>
    {
        private readonly IUnityContainer _container;

        public PaymentsViewModel(IUnityContainer container) {
            _container = container;
        }

        public PaymentTypeListView PaymentTypeList { get; set; }
        public PaymentListView PaymentList { get; set; }

        public PaymentTypeListViewModel PaymentTypeListViewModel { get; set; }

        private PaymentType _selectedPaymentType;
        public PaymentType SelectedPaymentType {
            get => _selectedPaymentType;
            set {
                _selectedPaymentType = value;
                if (value != null)
                    Remaining = value.Remaining ?? 0;
            }
        }


        public PaymentListViewModel PaymentListViewModel { get; set; }
        public Payment SelectedPayment { get; set; }

        public int Remaining { get; set; }

        public override async Task Initialize() {
            if (PaymentListViewModel == null)
                PaymentTypeListViewModel = _container.Resolve<PaymentTypeListViewModel>();
            await PaymentTypeListViewModel.Initialize();
            PaymentTypeListViewModel.OnSelectedPaymentTypeChanged += OnSelectedPaymentTypeChanged;
            PaymentTypeList = PaymentTypeListViewModel.GetView() as PaymentTypeListView;

            PaymentListViewModel = _container.Resolve<PaymentListViewModel>();
            await PaymentListViewModel.Initialize();
            PaymentListViewModel.OnSelectedPaymentChanged += OnSelectedPaymentChanged;
            PaymentListViewModel.OnPaymentChanged += ReloadPayments;
            PaymentList = PaymentListViewModel.GetView() as PaymentListView;
        }

        private void ReloadPayments(object sender, PaymentOperation paymentOperation) {
            OnSelectedPaymentTypeChanged(this, PaymentTypeListViewModel.SelectedPaymentType);
        }

        private void OnSelectedPaymentChanged(object sender, Payment e) => SelectedPayment = e;

        private void OnSelectedPaymentTypeChanged(object sender, PaymentType e) {
            IsBusy = false;
            BusyExecute(async () => {
                SelectedPaymentType = e;
                if (e == null)
                    return;

                await PaymentListViewModel.Initialize(e.Id);
                Remaining = await _container.Resolve<IPaymentTypeRepository>()
                    .GetRemaining(SelectedPaymentType.Id);
            });
        }
    }
}
