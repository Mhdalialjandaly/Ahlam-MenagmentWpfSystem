using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EyeClinic.WPF.Base;

namespace EyeClinic.WPF.AppServices.DialogService.InformationDialog
{
    internal class InformationDialogViewModel : BaseViewModel<InformationDialogView>
    {
        public UIElement ContentControl { get; set; }

        public event Action OnOk;

        public ICommand OkCommand => new RelayCommand(Ok);

        public void Ok() {
            OnOk?.Invoke();
        }

        public void ShowMessage(string message) {
            ContentControl = new TextBlock() {
                Text = message,
                FontSize = 22
            };
        }
    }
}
