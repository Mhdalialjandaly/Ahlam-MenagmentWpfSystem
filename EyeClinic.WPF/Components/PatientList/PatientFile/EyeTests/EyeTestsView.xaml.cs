using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EyeClinic.Core;
using EyeClinic.Core.Enums;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base.Extends;
using Unity;

namespace EyeClinic.WPF.Components.PatientList.PatientFile.EyeTests
{
    /// <summary>
    /// Interaction logic for EyeTestsView.xaml
    /// </summary>
    public partial class EyeTestsView : UserControl
    {
        public EyeTestsView() {
            InitializeComponent();
        }

        private void Previous_click(object sender, RoutedEventArgs e) {
            if (DataContext is EyeTestsViewModel viewModel) {
                ContainerHandler.Container.Resolve<IDialogService>().DisposeLastDialog();

                var tempList = viewModel.PatientFileViewModel.HighLightServices.ToList(); ;
                tempList.Reverse();

                foreach (var service in tempList) {
                    if (service is PatientService.EyeTests or
                        PatientService.Diagnosis or
                        PatientService.Prescriptions or
                        PatientService.Tests or
                        PatientService.NewGlass or
                        PatientService.LabTests or
                        PatientService.Operations or
                        PatientService.MedicalReports or
                        PatientService.SpecialNote)
                        continue;

                    viewModel.PatientFileViewModel.Navigate(service.ToString());
                    break;
                }
            }
        }

        private void next_click(object sender, RoutedEventArgs e) {
            if (DataContext is EyeTestsViewModel viewModel) {
                ContainerHandler.Container.Resolve<IDialogService>().DisposeLastDialog();
                viewModel.PatientFileViewModel.Navigate("Diagnosis");
            }
        }

        private void FinishVisitClick(object sender, RoutedEventArgs e) {
            if (Global.DeviceName.ToLower() == "reception") {
                ContainerHandler.Container.Resolve<INotificationService>()
                    .Error("لا يمكن حفظ الزيارة من جهاز الاستقبال");
                return;
            }
            if (DataContext is EyeTestsViewModel patientSick) {
                ContainerHandler.Container.Resolve<IDialogService>().DisposeLastDialog();
                patientSick.PatientFileViewModel.FinishVisit();
            }
        }
    }
}
