using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using EyeClinic.Core;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Home;
using EyeClinic.WPF.Components.Shell.Footer;
using EyeClinic.WPF.Components.Shell.Header;
using Unity;

namespace EyeClinic.WPF.Components.Shell
{
    public class ShellViewModel : BaseViewModel<ShellView>
    {
        private readonly IUnityContainer _container;
        private readonly IDialogService _dialogService;
        private readonly INotificationService _notificationService;

        public ShellViewModel(IUnityContainer container, IDialogService dialogService, INotificationService notificationService) {
            _container = container;
            _dialogService = dialogService;
            _notificationService = notificationService;
        }

        public override async Task Initialize() {
            var header = _container.Resolve<HeaderViewModel>();
            await header.Initialize();
            _container.RegisterInstance(header);
            Header = header.GetView() as HeaderView;

            var homeViewModel = _container.Resolve<HomeViewModel>();
            await homeViewModel.Initialize();
            Content = homeViewModel.GetView() as HomeView;

            var footer = _container.Resolve<FooterViewModel>();
            await footer.Initialize();
            _container.RegisterInstance(footer);
            Footer = footer.GetView() as FooterView;

            _container.RegisterInstance(homeViewModel);
            _container.RegisterInstance(footer);

            AddCommand = new RelayCommand(Add);

            await base.Initialize();

            if (Global.DeviceName == "Reception" || Global.DeviceName == "VS") {
                var timer = new DispatcherTimer() {
                    Interval = TimeSpan.FromSeconds(1)
                };
                timer.Tick += Timer_Tick;
                timer.Start();
            }

            var operationSessions = await File.ReadAllTextAsync(
                AppDomain.CurrentDomain.BaseDirectory + "//OperationSessions.txt");
            Global.OperationWithSessionCodes = operationSessions.Split(",");
        }


        private bool pending;
        private async void Timer_Tick(object sender, EventArgs e) {
            if (pending)
                return;
            if (await _container.Resolve<IAppLanguageRepository>().IsWaitingForNextPatient()) {
                pending = true;
                _container.Resolve<IDialogService>()
                        .ShowRedConfirmationMessage("الرجاء الانتباه  !!", async () => {
                            await _container.Resolve<IAppLanguageRepository>().AcceptNext();
                            pending = false;
                            return true;
                        }, () => pending = false);
            }
            if (await _container.Resolve<IAppLanguageRepository>().IsWaitingForNextPatient())
            {
                pending = true;
                _container.Resolve<IDialogService>()
                        .ShowRedConfirmationMessage("الرجاء ادخال المريض التالي !!", async () => {
                            await _container.Resolve<IAppLanguageRepository>().AcceptNext();
                            pending = false;

                            return true;
                        }, () => pending = false);
            }
        }


        private void Add() {
            _notificationService.Success("Adding item");
            _dialogService.ShowConfirmationMessage("You are here!!", () => Task.FromResult(true));
            _notificationService.Error("Failed");
        }

        public ICommand AddCommand { get; set; }


        public UserControl Dialog => (UserControl)_dialogService.GetView();

        public UserControl Header { get; set; }
        public UserControl Footer { get; set; }
        public UserControl Content { get; set; }
    }
}
