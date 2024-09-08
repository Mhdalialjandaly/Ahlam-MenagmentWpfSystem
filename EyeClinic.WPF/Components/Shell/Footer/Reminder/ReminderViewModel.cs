using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Setup;
using Unity;

namespace EyeClinic.WPF.Components.Shell.Footer.Reminder
{
    public partial class ReminderViewModel
    {
        public ReminderViewModel() {
            if (IsDesignMode) { }
        }
    }

    public partial class ReminderViewModel : BaseViewModel<ReminderView>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUnityContainer _container;

        public ReminderViewModel(IPaymentRepository paymentRepository, IUnityContainer container) {
            _paymentRepository = paymentRepository;
            _container = container;

            ReminderDate = DateTime.Now;
            RefreshCommand = new RelayCommand(Refresh);
            BackHomeCommand = new RelayCommand(BackHome);
        }

        public override async Task Initialize() {
            var view = GetView() as ReminderView;
            if (view == null)
                throw new Exception("Invalid Reminder View");

            ReminderDate = ReminderDate;

            Payments = await _paymentRepository
                .GetByKey(e => e.ReminderDate == ReminderDate.Date);
        }

        public DateTime ReminderDate { get; set; }

        public List<PaymentDto> Payments { get; set; }


        public ICommand RefreshCommand { get; set; }
        public ICommand BackHomeCommand { get; set; }

        private void Refresh() {
            BusyExecute(async () => {
                Payments = await _paymentRepository
                    .GetByKey(e => e.ReminderDate == ReminderDate.Date);
            });
        }

        private void BackHome() {
            BusyExecute(async () => {
                await _container.BackHome();
            });
        }
    }
}
