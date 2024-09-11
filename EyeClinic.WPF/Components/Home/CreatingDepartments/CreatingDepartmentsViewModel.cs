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
            if (target == "واجهة العملاء")
            {
                target = "Reception";
            }
            else if (target == "واجهة العمل الجماعي")
            {
                target = "Clinic";
            }
            else if (target == "تقارير")
            {
                target = "Reports";
            }
            else if (target == "واجهة المراسلات")
            {
                target = "AddressBook";
            }
            else if (target == "الكرتون")
            {
                target = "Payments";
            }
            else if (target == "واجهة التصنيع")
            {
                target = "Operations";
            }
            else if (target == "الماركات")
            {
                target = "PatientEyeImages";
            }
            else if (target == "الرجوع")
            {
                target = "OperationView";
            }


            switch (target)
            {
                case nameof(Services.Reception):

                    BusyExecute(async () => {
                        var reception = _container.Resolve<ReceptionViewModel>();
                        await reception.Initialize();
                        _container.Navigate(reception.GetView() as ReceptionView);
                    });
                    break;
                case nameof(Services.Clinic):

                    BusyExecute(async () => {
                        var clinic = _container.Resolve<ClinicViewModel>();
                        await clinic.Initialize();
                        _container.Navigate(clinic.GetView() as ClinicView);
                    });
                    break;
                case nameof(Services.Payments):

                    BusyExecute(async () =>
                    {
                        var cartoonView = _container.Resolve<CartoonViewModel>();
                        await cartoonView.Initialize();
                        _container.Navigate(cartoonView.GetView() as CartoonView);
                    });
                    break;
                case nameof(Services.Operations):
                    // var userod = Global.GetValue(GlobalKeys.LoggedUser);
                    //user = (UserDto)userod ;
                    //switch (user.RoleId)
                    //{
                    //    case 1:
                    //        BusyExecute(async () =>
                    //        {
                    //            var readyItems = _container.Resolve<ReadyItemsViewModel>();
                    //            await readyItems.Initialize();
                    //            _container.Navigate(readyItems.GetView() as ReadyItemsView);
                    //        });
                    //        break;
                    //    case 2:
                    //        BusyExecute(async () =>
                    //        {
                    //            var readyItems = _container.Resolve<ReadyItemsViewModel>();
                    //            await readyItems.Initialize();
                    //            _container.Navigate(readyItems.GetView() as ReadyItemsView);
                    //        });
                    //        break;
                    //    case 3:
                    //        BusyExecute(async () =>
                    //        {
                    //            var readyItems = _container.Resolve<ReadyItemsViewModel>();
                    //            await readyItems.Initialize();
                    //            _container.Navigate(readyItems.GetView() as ReadyItemsView);
                    //        });
                    //        break;
                    //    case 4:
                    //        BusyExecute(async () =>
                    //        {
                    //            var readyItems = _container.Resolve<ReadyItemsViewModel>();
                    //            await readyItems.Initialize();
                    //            _container.Navigate(readyItems.GetView() as ReadyItemsView);
                    //        });
                    //        break;
                    //    case 5:
                    //        BusyExecute(async () =>
                    //        {
                    //            var readyItems = _container.Resolve<ReadyItemsViewModel>();
                    //            await readyItems.Initialize();
                    //            _container.Navigate(readyItems.GetView() as ReadyItemsView);
                    //        });
                    //        break;
                    //    case 6:
                    //        BusyExecute(async () =>
                    //        {
                    //            var readyItems = _container.Resolve<ReadyItemsViewModel>();
                    //            await readyItems.Initialize();
                    //            _container.Navigate(readyItems.GetView() as ReadyItemsView);
                    //        });
                    //        break;
                    //}                   



                    break;
                case nameof(Services.PatientEyeImages):
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

                    BusyExecute(async () => {
                        var setting = _container.Resolve<SettingViewModel>();
                        await setting.Initialize();
                        _container.Navigate(setting.GetView() as SettingView);
                    });
                    break;
                case nameof(Services.AddressBook):

                    BusyExecute(async () => {
                        var setting = _container.Resolve<AddressBookViewModel>();
                        await setting.Initialize();
                        _container.Navigate(setting.GetView() as AddressBookView);
                    });
                    break;
                case nameof(Services.OperationView):

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
