using System;
using System.Windows.Input;
using EyeClinic.WPF.Base;

namespace EyeClinic.WPF.AppServices.DialogService.ConfirmationDialog
{
    public class ConfirmationDialogViewModel : BaseViewModel<ConfirmationDialogView>
    {
        public string Message { get; set; }

        public event Action OnYes;
        public event Action OnNo;

        public ICommand YesCommand => new RelayCommand(Yes);
        public ICommand NoCommand => new RelayCommand(No);

        private void No() {
            OnNo?.Invoke();
        }

        private void Yes() {
            OnYes?.Invoke();
        }
    }
}
