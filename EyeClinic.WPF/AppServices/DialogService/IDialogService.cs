using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using EyeClinic.WPF.Base.Interfaces;

namespace EyeClinic.WPF.AppServices.DialogService
{
    public interface IDialogService : IPresentable
    {
        public event Func<bool> OnDisposeDialog;

        void ShowInformationDialog(UserControl control, Action ok = null);
        void ShowInformationDialog(string message, Action ok = null);
        void ShowPopupContent(UserControl control, Action ok = null);
        void ShowFullScreenPopupContent(UserControl control);
        void ShowConfirmationMessage(string message, Func<Task<bool>> yes, Action no = null);
        void ShowRedConfirmationMessage(string message, Func<Task<bool>> yes, Action no = null);
        void ShowEditorDialog(UserControl control, Func<Task<bool>> save, Action cancel = null);
        void DisposeLastDialog();
        bool IsPopupActivated();
    }
}
