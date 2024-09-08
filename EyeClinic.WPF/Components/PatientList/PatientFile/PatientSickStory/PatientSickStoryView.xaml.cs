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
using EyeClinic.WPF.Components.PatientList.PatientFile.Prescriptions;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Unity;

namespace EyeClinic.WPF.Components.PatientList.PatientFile.PatientSickStory
{
    /// <summary>
    /// Interaction logic for PatientSickStoryView.xaml
    /// </summary>
    public partial class PatientSickStoryView : UserControl
    {
        public PatientSickStoryView()
        {
            InitializeComponent();
        }

        public string LoggedInUser { get; private set; }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is PatientSickStoryViewModel patientSick)
            {
                if (patientSick.BasePatientSpecialNote != patientSick.PatientSpecialNote)
                {
                    ContainerHandler.Container.Resolve<INotificationService>().Warning("Save your note before exit");
                    return;
                }

                ContainerHandler.Container.Resolve<IDialogService>().DisposeLastDialog();

                foreach (var service in patientSick.PatientFileViewModel.HighLightServices)
                {
                    if (service == PatientService.PatientFile)
                        continue;

                    patientSick.PatientFileViewModel.Navigate(service.ToString());
                    break;
                }
            }
        }

        private void FinishVisitClick(object sender, RoutedEventArgs e)
        {
            var user = Global.GetValue(GlobalKeys.LoggedUser);
            if (user is Model.UserDto loggedInUser)
                LoggedInUser = loggedInUser.UserName;
            if (LoggedInUser.ToLower() != "المسؤول")
            {

                ContainerHandler.Container.Resolve<INotificationService>()
                    .Error("لا يمكن انهاء الطلبية من قبل الاقسام غير المخصصة");
                return;
            }
            if (DataContext is PrescriptionsViewModel patientSick)
            {
                ContainerHandler.Container.Resolve<IDialogService>().DisposeLastDialog();
                patientSick.PatientFileViewModel.FinishVisit();
            }


        }
    }
}


   
    