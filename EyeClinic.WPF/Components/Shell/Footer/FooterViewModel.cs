using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using EyeClinic.Core;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Dialogs.PasswordInput;
using EyeClinic.WPF.Components.Home;
using EyeClinic.WPF.Components.Home.Setting;
using EyeClinic.WPF.Components.Management;
using EyeClinic.WPF.Components.Shell.Footer.Reminder;
using EyeClinic.WPF.Setup;
using Unity;

namespace EyeClinic.WPF.Components.Shell.Footer
{
    public class FooterViewModel : BaseViewModel<FooterView>
    {
        private readonly IUnityContainer _container;
        private readonly IDialogService _dialogService;
        private readonly INotificationService _notificationService;

        public FooterViewModel(IUnityContainer container, IDialogService dialogService, INotificationService notificationService) {
            _container = container;
            _dialogService = dialogService;
            _notificationService = notificationService;

            SettingsCommand = new RelayCommand<string>(Settings);
            ReminderCommand = new RelayCommand(Reminder);
            LogoutCommand = new RelayCommand(Logout);
        }

        public override async Task Initialize() {
            var user = Global.GetValue(GlobalKeys.LoggedUser);
            if (user is Model.UserDto loggedInUser)
                LoggedInUser = loggedInUser.UserName;

            ReminderViewModel = _container.Resolve<ReminderViewModel>();
            await ReminderViewModel.Initialize();

            TotalReminders = ReminderViewModel.Payments.Count;

            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += TimerOnTick;
            timer.Start();
        }

        private void TimerOnTick(object sender, EventArgs e) {
            CurrentLanguageCode = InputLanguageManager.Current.CurrentInputLanguage.DisplayName;
        }

        public ReminderViewModel ReminderViewModel { get; set; }

        public int TotalReminders { get; set; }
        public string LoggedInUser { get; set; }
        public string CurrentLanguageCode { get; set; }


        public ICommand LogoutCommand { get; set; }
        public ICommand SettingsCommand { get; set; }
        public ICommand ReminderCommand { get; set; }

        private void Settings(string target) {
            var loggedUser = Global.GetValue(GlobalKeys.LoggedUser);
            if (loggedUser == null)
            {
                var password = _container.Resolve<PasswordInputViewModel>();
                password.OnSuccessLogin += () => Settings(target);

                _container.Resolve<IDialogService>().
                    ShowPopupContent(password.GetView() as PasswordInputView);
                return;
            }
            if (!_container.CheckUserRole(UserRoles.Admin))
                return;
            if (LoggedInUser != null)
            {
                BusyExecute(async () => {
                    var settings = _container.Resolve<SettingViewModel>();
                    await settings.Initialize();

                    _container.Navigate(settings.GetView() as SettingView);
                });
            }
            else
            {                
                MessageBox.Show("PLease LogIn First");
            }
          
        }

        private void Reminder() {
            _container.Navigate(ReminderViewModel.GetView() as ReminderView);
        }

        private void Logout() {
            var loggedUser = Global.GetValue(GlobalKeys.LoggedUser);
            if (loggedUser == null)
                return;

            if (loggedUser is UserDto user) {
                _dialogService.ShowConfirmationMessage($"Logout from {user.UserName}", async () => {
                    Global.RemoveValue(GlobalKeys.LoggedUser);
                    LoggedInUser = string.Empty;

                    while (_dialogService.IsPopupActivated()) {
                        _dialogService.DisposeLastDialog();
                    }

                    await _container.BackHome();
                    return true;
                });
            }
        }
    }
}
