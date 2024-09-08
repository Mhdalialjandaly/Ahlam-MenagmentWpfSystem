using System.Windows.Controls;
using EyeClinic.WPF.Base;

namespace EyeClinic.WPF.AppServices.DialogService.PopupDialog
{
    public class PopupDialogViewModel : BaseViewModel<PopupDialogView>
    {
        public ContentControl ContentControl { get; set; }
    }
}
