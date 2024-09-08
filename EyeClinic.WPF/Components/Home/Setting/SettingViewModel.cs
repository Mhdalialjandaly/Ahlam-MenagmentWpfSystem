using System.Windows.Controls;
using System.Windows.Input;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Dialogs.PasswordInput;
using EyeClinic.WPF.Components.Home.Setting.AppSetting;
using EyeClinic.WPF.Components.Home.Setting.Diagnosis;
using EyeClinic.WPF.Components.Home.Setting.Diseases;
using EyeClinic.WPF.Components.Home.Setting.EyeTests;
using EyeClinic.WPF.Components.Home.Setting.LabTests;
using EyeClinic.WPF.Components.Home.Setting.Locations;
using EyeClinic.WPF.Components.Home.Setting.MedicalCenters;
using EyeClinic.WPF.Components.Home.Setting.Medicines;
using EyeClinic.WPF.Components.Home.Setting.MedicineUsage;
using EyeClinic.WPF.Components.Home.Setting.NotPayReason;
using EyeClinic.WPF.Components.Home.Setting.Operations;
using EyeClinic.WPF.Components.Home.Setting.Tests;
using EyeClinic.WPF.Components.Home.Setting.Users;
using EyeClinic.WPF.Components.Home.Setting.VisitCost;
using EyeClinic.WPF.Components.PatientList.PatientFile.Tests;
using EyeClinic.WPF.Setup;
using Unity;

namespace EyeClinic.WPF.Components.Home.Setting
{
    public partial class SettingViewModel
    {
        public SettingViewModel() { }
    }


    public partial class SettingViewModel : BaseViewModel<SettingView>
    {
        private readonly IUnityContainer _container;

        public SettingViewModel(IUnityContainer container) {
            _container = container;

            NavigateCommand = new RelayCommand<string>(Navigate);
            BackHomeCommand = new RelayCommand(BackHome);
        }

        public UserControl Content { get; set; }



        public ICommand NavigateCommand { get; set; }
        public ICommand BackHomeCommand { get; set; }

        private void Navigate(string target) {
            BusyExecute(async () => {
            if (target == "AppSetting") {
                var appSetting = _container.Resolve<AppSettingViewModel>();
                await appSetting.Initialize();
                Content = appSetting.GetView() as AppSettingView;
                return;
            }
            if (target == "Medicines") {
                var appSetting = _container.Resolve<MedicineViewModel>();
                await appSetting.Initialize();
                Content = appSetting.GetView() as MedicineView;
                return;
            }
            if (target == "MedicineUsage") {
                var appSetting = _container.Resolve<MedicineUsageViewModel>();
                await appSetting.Initialize();
                Content = appSetting.GetView() as MedicineUsageView;
                return;
            }
            if (target == "Diagnosis") {
                var appSetting = _container.Resolve<DiagnosisViewModel>();
                await appSetting.Initialize();
                Content = appSetting.GetView() as DiagnosisView;
                return;
            }
            if (target == "LabTests") {
                var appSetting = _container.Resolve<LabTestViewModel>();
                await appSetting.Initialize();
                Content = appSetting.GetView() as LabTestView;
                return;
            }
            if (target == "EyeTests") {
                var appSetting = _container.Resolve<EyeTestViewModel>();
                await appSetting.Initialize();
                Content = appSetting.GetView() as EyeTestView;
                return;
            }
            if (target == "Tests") {
                var appSetting = _container.Resolve<TestViewModel>();
                await appSetting.Initialize();
                Content = appSetting.GetView() as TestView;
                return;
            }
            if (target == "Diseases") {
                var diseaseManagement = _container.Resolve<DiseaseViewModel>();
                await diseaseManagement.Initialize();
                Content = diseaseManagement.GetView() as DiseaseView;
                return;
            }
            if (target == "Locations") {
                var appSetting = _container.Resolve<LocationViewModel>();
                await appSetting.Initialize();
                Content = appSetting.GetView() as LocationView;
                return;
            }
            if (target == "MedicalCenters") {
                var appSetting = _container.Resolve<MedicalCenterViewModel>();
                await appSetting.Initialize();
                Content = appSetting.GetView() as MedicalCenterView;
                return;
            }
            if (target == "Operations") {
                var appSetting = _container.Resolve<OperationViewModel>();
                await appSetting.Initialize();
                Content = appSetting.GetView() as OperationView;
                return;
            }
            if (target == "NotPayReasons") {
                var appSetting = _container.Resolve<NotPayReasonViewModel>();
                await appSetting.Initialize();
                Content = appSetting.GetView() as NotPayReasonView;
                return;
            }
            if (target == "VisitCost") {
                var appSetting = _container.Resolve<VisitCostViewModel>();
                await appSetting.Initialize();
                Content = appSetting.GetView() as VisitCostView;
                return;
            }
                if (target == "Users")
                {

                    //var pwd = _container.Resolve<PasswordInputViewModel>();
                    //pwd.DisposeOnLogin = false;
                    //pwd.CustomPassword = "54425";
                    //pwd.OnSuccessLogin += () =>
                    //{
                    //    BusyExecute(async () =>
                    //    {

                    var appSetting = _container.Resolve<UsersViewModel>();
                            await appSetting.Initialize();
                            Content = appSetting.GetView() as UsersView;
                            return;
                    //    });
                       
                    //};
                    //_container.Resolve<IDialogService>()
                    //    .ShowPopupContent(pwd.GetView() as PasswordInputView);
                };
                if (target == "DeniedUsers")
                {        
                            var appSetting = _container.Resolve<UsersViewModel>();
                            await appSetting.Initialize();
                            Content = appSetting.GetView() as UsersView;
                            return;
                };
            });
        }

        private void BackHome() {
            BusyExecute(async () => {
                await _container.BackHome();
            });
        }
    }
}
