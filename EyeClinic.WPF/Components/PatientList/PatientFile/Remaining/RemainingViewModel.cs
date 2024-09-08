using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using EyeClinic.Core;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Base.Extends;
using EyeClinic.WPF.Components.Dialogs.PasswordInput;
using EyeClinic.WPF.Components.PatientList.PatientFile.PaymentForRemaining;
using EyeClinic.WPF.Components.PatientList.PatientFile.Remaining;
using Unity;

namespace EyeClinic.WPF.Components.PatientList.PatientFile.Remaining
{
    public partial class RemainingViewModel
    {
        public RemainingViewModel()
        {

        }
    }

    public partial class RemainingViewModel : BaseViewModel<RemainingView>
    {
        private readonly IPatientOperationRepository _patientOperationRepository;
        private readonly IPatientVisitRepository _patientVisitRepository;
        private readonly IDialogService _dialogService;
        private readonly IUnityContainer _container;

        private readonly IDeniedUserRepository _deniedUserRepository;
        private readonly INotificationService _notificationService;

        public RemainingViewModel(IPatientRepository patientRepository,
                IPatientOperationRepository patientOperationRepository
            , IUnityContainer unityContainer
            , IDialogService dialogService,
                IPatientVisitRepository patientVisitRepository, IDeniedUserRepository deniedUserRepository, INotificationService notificationService)
        {
            _patientOperationRepository = patientOperationRepository;
            _patientVisitRepository = patientVisitRepository;
            _dialogService = dialogService;
            _container = unityContainer;
            int Counter = 0;
            var user = Global.GetValue(GlobalKeys.LoggedUser);

            AddPaymentCommand = new RelayCommand(async () =>
            {

                //var user = Global.GetValue(GlobalKeys.LoggedUser);
                //if (user is Model.UserDto usermodel)
                //{
                //    var denieduser = await _deniedUserRepository.GetByKey(e => e.UserName == usermodel.UserName);
                //    if (!denieduser.IsNullOrEmpty())
                //    {
                //        Show toaster
                //        _notificationService.Error("Your Account  is Banned");
                //        return;
                //    }
                //}
                VisitDate = await _patientVisitRepository
               .GetVisitDateById(PatientVisitId);
             
                if (VisitDate == DateTime.Today)
                {
                    var pwd = _container.Resolve<PasswordInputViewModel>();

                    pwd.DisposeOnLogin = false;
                    Counter = Counter + 1;
                    pwd.CustomPassword = "54425";
                    pwd.OnSuccessLogin += () =>
                    {
                        _dialogService.DisposeLastDialog();

                        var payment = _container.Resolve<PaymentForRemainingViewModel>();
                        _dialogService.ShowEditorDialog(payment.GetView() as PaymentForRemainingView,
                            async () =>
                            {
                                if (payment.Payment > 0)
                                {
                                    await _patientVisitRepository.AddPayment(PatientId, payment.Payment);
                                    await _patientVisitRepository.AddPaymentForToday(PatientId, payment.Payment);
                                    PatientRemaining = PatientRemaining - payment.Payment;

                                    OnAddPayment?.Invoke(this, PatientRemaining);
                                    return true;
                                }
                                return false;
                            });
                    };
                    _container.Resolve<IDialogService>()
                        .ShowPopupContent(pwd.GetView() as PasswordInputView);
                }
                else
                {
                    MessageBox.Show("The Visit Date Most Same Date Of Today.......");
                    //_notificationService.Error("The Visit Date Most Same Date Of Today .......");
                }
            });

            AddOperationPaymentCommand = new RelayCommand(() =>
            {
                var pwd = _container.Resolve<PasswordInputViewModel>();
                pwd.CustomPassword = "54425";
                pwd.DisposeOnLogin = false;
                pwd.OnSuccessLogin += () =>
                {
                    _dialogService.DisposeLastDialog();

                    var payment = _container.Resolve<PaymentForRemainingViewModel>();
                    _dialogService.ShowEditorDialog(payment.GetView() as PaymentForRemainingView,
                        async () =>
                        {
                            if (payment.Payment > 0)
                            {
                                await _patientOperationRepository.AddPayment(PatientId, payment.Payment);
                                OperationRemaining -= payment.Payment;
                                return true;
                            }

                            return false;
                        });
                };
                _container.Resolve<IDialogService>()
                    .ShowPopupContent(pwd.GetView() as PasswordInputView);
            });

        }

        public async Task InitializeAsync(int patientId, int Remaining, int operationRemaining,int patientvisitid)
        {
            PatientVisitId = patientvisitid;
            PatientId = patientId;
            PatientRemaining = Remaining;
            OperationRemaining = operationRemaining;

            VisitDate = await _patientVisitRepository
             .GetVisitDateById(PatientVisitId);
        }

        public event EventHandler<int> OnAddPayment;

        public bool IfRemaining { get; set; }
        public int PatientRemaining { get; set; }
        public int PatientRemainPayValue { get; set; }
        public int PatientVisitId { get; set; }
        public DateTime? VisitDate { get; set; }
        public DateTime? SelectedVisitDate { get; set; }
        public int OperationRemaining { get; set; }
        public int PatientId { get; set; }



        public ICommand AddPaymentCommand { get; set; }
        public ICommand AddOperationPaymentCommand { get; set; }

        public ICommand CancelCommand => new RelayCommand(() =>
        {
            ContainerHandler.Container.Resolve<IDialogService>().DisposeLastDialog();
        });


    }
}

