using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using EyeClinic.Core;
using EyeClinic.WPF.AppServices.DialogService.ConfirmationDialog;
using EyeClinic.WPF.AppServices.DialogService.EditorDialog;
using EyeClinic.WPF.AppServices.DialogService.FullScreenPopup;
using EyeClinic.WPF.AppServices.DialogService.InformationDialog;
using EyeClinic.WPF.AppServices.DialogService.PopupDialog;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;

namespace EyeClinic.WPF.AppServices.DialogService
{
    public class DialogServiceViewModel : BaseViewModel<DialogServiceView>, IDialogService
    {
        public readonly INotificationService _notificationService;
        public DialogServiceViewModel(INotificationService notificationService) { _notificationService = notificationService; }
        public ObservableCollection<UserControl> Dialogs { get; set; }

        public void ShowInformationDialog(UserControl control, Action ok = null) {
            var dContext = new InformationDialogViewModel { ContentControl = control };
            dContext.OnOk += () => {
                ok?.Invoke();
                DisposeLastDialog();
            };
            var dialogHost = new DialogHostViewModel { Content = new InformationDialogView { DataContext = dContext } };
            ViewDialog(dialogHost);
        }

        public void ShowInformationDialog(string message, Action ok = null) {
            var dContext = new InformationDialogViewModel();
            dContext.ShowMessage(message);
            dContext.OnOk += () => {
                ok?.Invoke();
                DisposeLastDialog();
            };
            var dialogHost = new DialogHostViewModel { Content = new InformationDialogView { DataContext = dContext } };
            ViewDialog(dialogHost);
        }

        public void ShowPopupContent(UserControl control, Action ok = null) {
            var dContext = new PopupDialogViewModel { ContentControl = control };
            var dialogHost = new DialogHostViewModel {
                Content = new PopupDialogView { DataContext = dContext },
                Background = "Transparent"
            };
            ViewDialog(dialogHost);
        }

        public void ShowFullScreenPopupContent(UserControl control) {
            var dContext = new FullScreenPopupViewModel { ContentControl = control };
            Dialogs ??= new ObservableCollection<UserControl>();
            Dialogs.Add(dContext.GetView() as FullScreenPopupView);
        }

        public void ShowConfirmationMessage(string message, Func<Task<bool>> yes, Action no = null) {
            var dContext = new ConfirmationDialogViewModel { Message = message };
            dContext.OnNo += () => {
                no?.Invoke();
                DisposeLastDialog();
            };
            dContext.OnYes += async () => {
                if (await yes.Invoke())
                    DisposeLastDialog();
            };
            var dialogHost = new DialogHostViewModel { Content = new ConfirmationDialogView { DataContext = dContext } };
            ViewDialog(dialogHost);
        }

        public void ShowRedConfirmationMessage(string message, Func<Task<bool>> yes, Action no = null) {
            var dContext = new ConfirmationDialogViewModel { Message = message };
            dContext.OnNo += () => {
                no?.Invoke();
                DisposeLastDialog();
            };
            dContext.OnYes += async () => {
                if (await yes.Invoke())
                    DisposeLastDialog();
            };
            var dialogHost = new DialogHostViewModel { Content = new RedConfirmationDialogView { DataContext = dContext } };
            ViewDialog(dialogHost);
        }

        public void ShowEditorDialog(UserControl control, Func<Task<bool>> save, Action cancel = null) {
            var dContext = new EditorDialogViewModel() { ContentControl = control };
            dContext.OnCancel += () => {
                ShowConfirmationMessage("Are You Sure :", () => {
                    cancel?.Invoke();
                    DisposeLastDialog();
                    return Task.FromResult(true);
                });
            };
            try
            {
                dContext.OnSave += async () => {
                    if (await save.Invoke())
                        DisposeLastDialog();
                };

            }
            catch (Exception e)
            {
                _notificationService.Warning(e.Message);                
            }  
            
            var dialogHost = new DialogHostViewModel { Content = new EditorDialogView() { DataContext = dContext } };
            ViewDialog(dialogHost);
        }

        public void ViewDialog(DialogHostViewModel dialogHostViewModel) {
            Dialogs ??= new ObservableCollection<UserControl>();
            Dialogs.Add(new DialogHostView { DataContext = dialogHostViewModel });
        }

        public event Func<bool> OnDisposeDialog;

        public void DisposeLastDialog() {
            if (!IsPopupActivated())
                return;

            if (OnDisposeDialog != null) {
                if (OnDisposeDialog.Invoke()) {
                    Dialogs.Remove(Dialogs.Last());
                }
            } else {
                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    Dialogs.Remove(Dialogs.Last());
                });
             
            }
        }

        public bool IsPopupActivated() {
            return !Dialogs.IsNullOrEmpty();
        }
    }
}
