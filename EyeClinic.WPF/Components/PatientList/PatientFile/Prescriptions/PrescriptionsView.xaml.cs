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
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base.Extends;
using Unity;

namespace EyeClinic.WPF.Components.PatientList.PatientFile.Prescriptions
{
    /// <summary>
    /// Interaction logic for PrescriptionsView.xaml
    /// </summary>
    public partial class PrescriptionsView : UserControl
    {
        public string LoggedInUser { get; private set; }

        public PrescriptionsView() {
            InitializeComponent();
        }

        private void MedicineList_DoubleClick(object sender, MouseButtonEventArgs e) {
            if (e.OriginalSource is Border border) {
                if (border.DataContext is MedicineDto result) {
                    if (DataContext is PrescriptionsViewModel viewModel) {
                        viewModel.AddMedicine();
                    }
                }
            } else {
                if (e.OriginalSource is TextBlock textBlock) {
                    if (textBlock.DataContext is MedicineDto result) {
                        if (DataContext is PrescriptionsViewModel viewModel) {
                            viewModel.AddMedicine();
                        }
                    }
                }
            }
        }

        private void MedicineUsage_DoubleClick(object sender, MouseButtonEventArgs e) {
            if (e.OriginalSource is Border border) {
                if (border.DataContext is MedicineUsageDto result) {
                    if (DataContext is PrescriptionsViewModel viewModel) {
                        viewModel.AddUsage();
                    }
                }
            } else {
                if (e.OriginalSource is TextBlock textBlock) {
                    if (textBlock.DataContext is MedicineUsageDto result) {
                        if (DataContext is PrescriptionsViewModel viewModel) {
                            viewModel.AddUsage();
                        }
                    }
                }
            }
        }

        private void Previous_click(object sender, RoutedEventArgs e) {
            if (DataContext is PrescriptionsViewModel viewModel) {
                ContainerHandler.Container.Resolve<IDialogService>().DisposeLastDialog();

                var tempList = viewModel.PatientFileViewModel.HighLightServices.ToList(); ;
                tempList.Reverse();

                foreach (var service in tempList) {
                    if (service is PatientService.Prescriptions or
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
            if (DataContext is PrescriptionsViewModel viewModel) {
                ContainerHandler.Container.Resolve<IDialogService>().DisposeLastDialog();
                foreach (var service in viewModel.PatientFileViewModel.HighLightServices) {
                    if (service is PatientService.PatientFile
                        or PatientService.EyeTests
                        or PatientService.Diagnosis
                        or PatientService.Prescriptions)
                        continue;

                    viewModel.PatientFileViewModel.Navigate(service.ToString());
                    break;
                }
            }
        }

        private void FinishVisitClick(object sender, RoutedEventArgs e) {
            var user = Global.GetValue(GlobalKeys.LoggedUser);
            if (user is Model.UserDto loggedInUser)
                LoggedInUser = loggedInUser.UserName;
            if (LoggedInUser.ToLower() != "المسؤول") {

                ContainerHandler.Container.Resolve<INotificationService>()
                    .Error("لا يمكن انهاء الطلبية من قبل الاقسام غير المخصصة");
                return;
            }
            if (DataContext is PrescriptionsViewModel patientSick) {
                ContainerHandler.Container.Resolve<IDialogService>().DisposeLastDialog();
                patientSick.PatientFileViewModel.FinishVisit();
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((PrescriptionsViewModel)this.DataContext).SearchCommand.Execute(null);
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            ((PrescriptionsViewModel)this.DataContext).SearchUsageCommand.Execute(null);
        }

        private void PaymentTypeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
