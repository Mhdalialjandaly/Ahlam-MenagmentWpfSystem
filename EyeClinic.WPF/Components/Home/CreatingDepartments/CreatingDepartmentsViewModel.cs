using EyeClinic.Core.Enums;
using EyeClinic.Core;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Dialogs.PasswordInput;
using EyeClinic.WPF.Components.Home.AddressBook;
using EyeClinic.WPF.Components.Home.CartoonForm;
using EyeClinic.WPF.Components.Home.Clinic;
using EyeClinic.WPF.Components.Home.Markets;
using EyeClinic.WPF.Components.Home.Reception;
using EyeClinic.WPF.Components.Home.Reports;
using EyeClinic.WPF.Components.Home.Setting;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Unity;
using EyeClinic.WPF.DataModel;
using EyeClinic.WPF.Setup;
using EyeClinic.WPF.Components.Home.CreatingDepartments.CreatingReport;

namespace EyeClinic.WPF.Components.Home.CreatingDepartments
{   
    public class CreatingDepartmentsViewModel : BaseViewModel<CreatingDepartmentsView>
    {
        private readonly IUnityContainer _container;

        public CreatingDepartmentsViewModel(IUnityContainer container)
        {
            _container = container;
            NavigateCommand = new RelayCommand<string>(Navigate);
        }

        public override Task Initialize()
        {
            CreatingNavigationList = CreatingNavigation.Create();
            return base.Initialize();
        }

        public List<CreatingNavigation> CreatingNavigationList { get; set; }
        public UserDto user { get; set; }

        public ICommand NavigateCommand { get; set; }


        private void Navigate(string target)
        {
            var loggedUser = Global.GetValue(GlobalKeys.LoggedUser);

            if (loggedUser == null)
            {
                var password = _container.Resolve<PasswordInputViewModel>();
                password.OnSuccessLogin += () => Navigate(target);

                _container.Resolve<IDialogService>().
                    ShowPopupContent(password.GetView() as PasswordInputView);
                return;
            }           


            switch (target)
            {
                case nameof(CreatingServices.التحضير1):

                    BusyExecute(async () => {
                        var reception = _container.Resolve<CreatingReportViewModel>();
                        await reception.Initialize();
                        _container.Navigate(reception.GetView() as CreatingReportView);
                    });
                    break;
                case nameof(CreatingServices.التحضير2):

                    BusyExecute(async () => {
                        var clinic = _container.Resolve<ClinicViewModel>();
                        await clinic.Initialize();
                        _container.Navigate(clinic.GetView() as ClinicView);
                    });
                    break;
                case nameof(CreatingServices.الحلاوة):

                    BusyExecute(async () =>
                    {
                        var cartoonView = _container.Resolve<CartoonViewModel>();
                        await cartoonView.Initialize();
                        _container.Navigate(cartoonView.GetView() as CartoonView);
                    });
                    break;
                case nameof(CreatingServices.زيت):
                            

                    break;
                case nameof(CreatingServices.طبخ):
                    BusyExecute(async () => {
                        var eyeImages = _container.Resolve<MarketsViewModel>();
                        await eyeImages.Initialize();
                        _container.Navigate(eyeImages.GetView() as MarketsView);
                    });
                    break;
                case nameof(CreatingServices.لبنة):
                    if (!_container.CheckUserRole(UserRoles.Reporter, UserRoles.Admin))
                        return;
                    var report = _container.Resolve<ReportsViewModel>();
                    _container.Navigate(report.GetView() as ReportsView);
                    break;
                case nameof(CreatingServices.مربى):

                    BusyExecute(async () => {
                        var setting = _container.Resolve<SettingViewModel>();
                        await setting.Initialize();
                        _container.Navigate(setting.GetView() as SettingView);
                    });
                    break;
                case nameof(CreatingServices.طحينة):

                    BusyExecute(async () => {
                        var setting = _container.Resolve<AddressBookViewModel>();
                        await setting.Initialize();
                        _container.Navigate(setting.GetView() as AddressBookView);
                    });
                    break;
                case nameof(CreatingServices.معلبات):

                    BusyExecute(async () => {
                        var setting = _container.Resolve<HomeViewModel>();
                        await setting.Initialize();
                        _container.Navigate(setting.GetView() as HomeView);
                    });
                    break;
                case nameof(CreatingServices.الزيتون):

                    BusyExecute(async () => {
                        var setting = _container.Resolve<CreatingReportViewModel>();
                        await setting.Initialize(target);
                        _container.Navigate(setting.GetView() as CreatingReportView);
                    });
                    break;
                case nameof(CreatingServices.خروج):

                    BusyExecute(async () => {
                        var setting = _container.Resolve<HomeViewModel>();
                        await setting.Initialize();
                        _container.Navigate(setting.GetView() as HomeView);
                    });
                    break;

            }
        }

    }
}
