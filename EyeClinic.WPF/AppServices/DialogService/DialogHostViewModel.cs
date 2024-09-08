using System.Windows;
using EyeClinic.WPF.Base;

namespace EyeClinic.WPF.AppServices.DialogService
{
    public class DialogHostViewModel : BaseViewModel<DialogHostView>
    {
        public DialogHostViewModel() {
            Height = "Auto";
            Background = "WhiteSmoke";
        }

        public UIElement Content { get; set; }
        public string Height { get; set; }
        public string Background { get; set; }
    }
}
