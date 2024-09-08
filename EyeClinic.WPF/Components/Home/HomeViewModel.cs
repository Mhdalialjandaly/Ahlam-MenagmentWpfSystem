using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using EyeClinic.Core;
using EyeClinic.Core.Enums;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Dialogs.PasswordInput;
using EyeClinic.WPF.Components.Home.AddressBook;
using EyeClinic.WPF.Components.Home.CartoonForm;
using EyeClinic.WPF.Components.Home.Clinic;
using EyeClinic.WPF.Components.Home.EyeImages;
using EyeClinic.WPF.Components.Home.Markets;
using EyeClinic.WPF.Components.Home.Operation;
using EyeClinic.WPF.Components.Home.OperationSchedule;
using EyeClinic.WPF.Components.Home.ReadyItems;
using EyeClinic.WPF.Components.Home.Reception;
using EyeClinic.WPF.Components.Home.Reports;
using EyeClinic.WPF.Components.Home.Setting;
using EyeClinic.WPF.Components.PatientList.PatientFile.Operations;
using EyeClinic.WPF.Components.Reports;
using EyeClinic.WPF.DataModel;
using EyeClinic.WPF.Setup;
using Unity;

namespace EyeClinic.WPF.Components.Home
{
    public class HomeViewModel : BaseViewModel<HomeView>
    {
        private readonly IUnityContainer _container;

        public HomeViewModel(IUnityContainer container) {
            _container = container;
            NavigateCommand = new RelayCommand<string>(Navigate);
        }

        public override Task Initialize() {
            NavigationList = Navigation.Create();
            return base.Initialize();
        }

        public List<Navigation> NavigationList { get; set; }


        public ICommand NavigateCommand { get; set; }


        private void Navigate(string target) {
            var loggedUser = Global.GetValue(GlobalKeys.LoggedUser);

            if (loggedUser == null) {
                var password = _container.Resolve<PasswordInputViewModel>();
                password.OnSuccessLogin += () => Navigate(target);

                _container.Resolve<IDialogService>().
                    ShowPopupContent(password.GetView() as PasswordInputView);
                return;
            }
            if (target== "واجهة العملاء")
            {
                target = "Reception";
            }
            else if (target == "واجهة العمل الجماعي")
            {
                target = "Clinic";
            }
            else if(target == "تقارير")
            {
                target = "Reports";
            }
            else if (target == "واجهة المراسلات")
            {
                target = "AddressBook";
            }
            else if(target == "الكرتون")
            {
                target = "Payments";
            }
            else if (target== "واجهة التصنيع")
            {
                target = "Operations";
            }
            else if (target == "الماركات")
            {
                target = "PatientEyeImages";
            }
            else if (target == "العمليات") 
            {
                target = "OperationView";
            }


            switch (target) {
                case nameof(Services.Reception):
                    if (!_container.CheckUserRole(UserRoles.Seller,UserRoles.Admin))
                        return;
                    BusyExecute(async () => {
                        var reception = _container.Resolve<ReceptionViewModel>();
                        await reception.Initialize();
                        _container.Navigate(reception.GetView() as ReceptionView);
                    });
                    break;
                case nameof(Services.Clinic):
                    if (!_container.CheckUserRole(UserRoles.Seller
                        ,UserRoles.Admin
                        ,UserRoles.Administrative
                        ,UserRoles.Designer))
                        return;
                    BusyExecute(async () => {
                        var clinic = _container.Resolve<ClinicViewModel>();
                        await clinic.Initialize();
                        _container.Navigate(clinic.GetView() as ClinicView);
                    });
                    break;
                case nameof(Services.Payments):
                    if (!_container.CheckUserRole(UserRoles.Cartoon,UserRoles.Admin,UserRoles.Administrative))
                        return;
                    BusyExecute(async () => 
                    {
                        var cartoonView = _container.Resolve<CartoonViewModel>();
                        await cartoonView.Initialize();
                        _container.Navigate(cartoonView.GetView() as CartoonView);
                    });
                    break;
                case nameof(Services.Operations):
                    if (!_container.CheckUserRole(UserRoles.Producters,UserRoles.Admin,UserRoles.Designer, UserRoles.Administrative,UserRoles.Seller))
                        return;
                   
                        BusyExecute(async () =>
                        {
                            var readyItems = _container.Resolve<ReadyItemsViewModel>();
                            await readyItems.Initialize();
                            _container.Navigate(readyItems.GetView() as ReadyItemsView);
                        });
                    
                    break;
                case nameof(Services.PatientEyeImages):
                    if (!_container.CheckUserRole(UserRoles.Admin,UserRoles.Marketer))
                        return;
                    BusyExecute(async () => {
                        var eyeImages = _container.Resolve<MarketsViewModel>();
                        await eyeImages.Initialize();
                        _container.Navigate(eyeImages.GetView() as MarketsView);
                    });
                    break;
                case nameof(Services.Reports):
                    if (!_container.CheckUserRole(UserRoles.Reporter, UserRoles.Admin))
                        return;                    
                    var report = _container.Resolve<ReportsViewModel>();
                        _container.Navigate(report.GetView() as ReportsView);                 
                    break;
                case nameof(Services.Settings):
                    if (!_container.CheckUserRole(UserRoles.Admin))
                        return;
                    BusyExecute(async () => {
                        var setting = _container.Resolve<SettingViewModel>();
                        await setting.Initialize();
                        _container.Navigate(setting.GetView() as SettingView);
                    });
                    break;
                case nameof(Services.AddressBook):
                    if (!_container.CheckUserRole(UserRoles.Sender, UserRoles.Admin))
                        return;
                    BusyExecute(async () => {
                        var setting = _container.Resolve<AddressBookViewModel>();
                        await setting.Initialize();
                        _container.Navigate(setting.GetView() as AddressBookView);
                    });
                    break;
                case nameof(Services.OperationView):
                    if (!_container.CheckUserRole(UserRoles.AdministrativeAssistant, UserRoles.Admin))
                        return;
                    BusyExecute(async () => {
                        var setting = _container.Resolve<OperationViewModel>();
                        await setting.Initialize();
                        _container.Navigate(setting.GetView() as OperationView);
                    });
                    break;

            }
        }

    }
}
