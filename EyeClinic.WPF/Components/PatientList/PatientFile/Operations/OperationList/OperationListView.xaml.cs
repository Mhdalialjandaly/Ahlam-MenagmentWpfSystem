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
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base.Extends;
using EyeClinic.WPF.Components.PatientList.PatientFile.PatientSickStory;
using Unity;

namespace EyeClinic.WPF.Components.PatientList.PatientFile.Operations.OperationList
{
    /// <summary>
    /// Interaction logic for OperationListView.xaml
    /// </summary>
    public partial class OperationListView : UserControl
    {
        public OperationListView() {
            InitializeComponent();
        }

        private void FinishVisitClick(object sender, RoutedEventArgs e) {
            if (Global.DeviceName.ToLower() == "reception") {
                ContainerHandler.Container.Resolve<INotificationService>()
                    .Error("لا يمكن حفظ الزيارة من جهاز الاستقبال");
                return;
            }
            if (DataContext is OperationListViewModel patientSick) {
                ContainerHandler.Container.Resolve<IDialogService>().DisposeLastDialog();
                patientSick.PatientFileViewModel.FinishVisit();
            }
        }

        private void StackPanel_Click(object sender, RoutedEventArgs e)
        {
       
        }

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Control_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
