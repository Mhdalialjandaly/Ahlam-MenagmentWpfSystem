using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.PatientList.PatientVisitList.Why;

namespace EyeClinic.WPF.AppServices.DialogService.EditorDialog
{
    public class EditorDialogViewModel : BaseViewModel<EditorDialogView>
    {
      
        public event Action OnSave;
        public event Action OnCancel;
        INotificationService notification;

        public UIElement ContentControl { get; set; }

        public ICommand SaveCommand => new RelayCommand(Save, _ => IsBusy == false);
        public ICommand CancelCommand => new RelayCommand(Cancel, _ => IsBusy == false);
        int counter;
        private  void Save() {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                OnSave?.Invoke();
            });
        }

        private void Cancel() {
            OnCancel?.Invoke();
        }
    }
}
