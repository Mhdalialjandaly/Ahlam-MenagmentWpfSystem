using EyeClinic.WPF.Base;
using System.Windows.Input;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.Components.Dialogs.PasswordInput;
using EyeClinic.WPF.Components.Home.Reports.ClinicIncomeReport;
using EyeClinic.WPF.Components.Home.Reports.NamesOfPatientsWithASpecificDiseaseCondition;
using EyeClinic.WPF.Components.Home.Reports.NumberOfPatients;
using EyeClinic.WPF.Components.Home.Reports.PatientsWhoHaveHadSurgeries;
using EyeClinic.WPF.Setup;
using Unity;
using EyeClinic.WPF.Components.Home.Reports.EnteringTheOperation;
using EyeClinic.WPF.Components.Home.Reports.PatientByMedicineReport;
using EyeClinic.WPF.Components.Home.Reports.PatientVisitTestReport;
using EyeClinic.WPF.Components.Home.Reports.RemainingReports;
using EyeClinic.WPF.Components.Reports.NumberOfGlassesPrescribed;
using EyeClinic.WPF.Components.Home.Reports.TotalLensReport;

namespace EyeClinic.WPF.Components.Home.Reports
{
    public class ReportsViewModel : BaseViewModel<ReportsView>
    {
        private readonly IUnityContainer _container;

        public ReportsViewModel(IUnityContainer container) {
            _container = container;
            ShowReport = new RelayCommand<string>(ShowReportMethod);

            BackCommand = new RelayCommand(() => {
                BusyExecute(async () => await container.BackHome());
            });
        }
        public ICommand ShowReport { get; set; }
        public ICommand BackCommand { get; set; }

        public void ShowReportMethod(string target) {
            var pwd = _container.Resolve<PasswordInputViewModel>();
            pwd.CustomPassword = "54425";
            pwd.OnSuccessLogin += () => {
                _container.Resolve<IDialogService>().DisposeLastDialog();
                switch (target) {
                    case "PNSD":
                        BusyExecute(async () => {
                            var reportWindow = _container.Resolve<NamesOfPatientsWithASpecificDiseaseConditionViewModel>();
                            await reportWindow.Initialize();
                            (reportWindow.GetView() as NamesOfPatientsWithASpecificDiseaseConditionView)?.ShowDialog();
                        });
                        break;
                    case "PNSO":
                        BusyExecute(async () => {
                            var reportWindow = _container.Resolve<PatientsWhoHaveHadSurgeriesViewModel>();
                            await reportWindow.Initialize();
                            (reportWindow.GetView() as PatientsWhoHaveHadSurgeriesView)?.ShowDialog();
                        });
                        break;
                    case "Medicine":
                        BusyExecute(async () => {
                            var reportWindow = _container.Resolve<PatientByMedicineReportViewModel>();
                            await reportWindow.Initialize();
                            (reportWindow.GetView() as PatientByMedicineReportView)?.ShowDialog();
                        });
                        break;
                    case "TotalPatient":
                        BusyExecute(async () => {
                            var reportWindow = _container.Resolve<NumberOfPatientsViewModel>();
                            await reportWindow.Initialize();
                            (reportWindow.GetView() as NumberOfPatientsView)?.ShowDialog();
                        });
                        break;
                    case "TotalPatientByGender":
                        BusyExecute(async () => {
                            var reportWindow = _container.Resolve<PatientVisitTestReportViewModel>();
                            await reportWindow.Initialize();
                            (reportWindow.GetView() as PatientVisitTestReportView)?.ShowDialog();
                        });
                        break;
                    case "ReferralCasesReport":
                        BusyExecute(async () => {
                            var reportWindow = _container.Resolve<PatientVisitTestReportViewModel>();
                            await reportWindow.Initialize();
                            (reportWindow.GetView() as PatientVisitTestReportView)?.ShowDialog();
                        });
                        break;
                    case "RemainigReport":
                        BusyExecute(async () => {
                            var reportWindow = _container.Resolve<RemainingReportsViewModel>();
                            await reportWindow.Initialize();
                            (reportWindow.GetView() as RemainingReportsView)?.ShowDialog();
                        });
                        break;
                    case "SpecialNote":
                        break;
                    case "NameOfPatients":
                        BusyExecute(async () => {
                            var reportWindow = _container.Resolve<PatientVisitTestReportViewModel>();
                            await reportWindow.Initialize();
                            (reportWindow.GetView() as PatientVisitTestReportView)?.ShowDialog();
                        });
                        break;
                    case "TotalPatientByLocationReport":
                        BusyExecute(async () => {
                            var reportWindow = _container.Resolve<EnteringTheOperationViewModel>();
                            await reportWindow.Initialize();
                            (reportWindow.GetView() as EnteringTheOperationView)?.ShowDialog();
                        });
                        break;
                    case "OperationRevenue":
                        BusyExecute(async () => {
                            var reportWindow = _container.Resolve<EnteringTheOperationViewModel>();
                            await reportWindow.Initialize();
                            (reportWindow.GetView() as EnteringTheOperationView)?.ShowDialog();
                        });
                        break;
                    case "ClinicIncomeReport":
                        BusyExecute(async () => {
                            var reportWindow = _container.Resolve<ClinicIncomeReportViewModel>();
                            await reportWindow.Initialize();
                            (reportWindow.GetView() as ClinicIncomeReportView)?.ShowDialog();
                        });
                        break;
                    case "TotalPatientByGlass":
                        BusyExecute(async () => {
                            var reportWindow = _container.Resolve<TotalLensReportViewModel>();
                            await reportWindow.Initialize();
                            (reportWindow.GetView() as TotalLensReportView)?.ShowDialog();
                        });
                        break;
                    default:
                        break;
                }
            };

            _container.Resolve<IDialogService>()
                    .ShowPopupContent(pwd.GetView() as PasswordInputView);
        }
    }
}
