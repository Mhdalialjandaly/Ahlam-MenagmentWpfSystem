using System.Windows.Controls;
using System.Windows.Input;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Base.Extends;
using EyeClinic.WPF.Components.PatientList.PatientFile.FinishVisit;
using Unity;

namespace EyeClinic.WPF.AppServices.DialogService.FullScreenPopup
{
    public class FullScreenPopupViewModel : BaseViewModel<FullScreenPopupView>
    {
        public ContentControl ContentControl { get; set; }
        

        public ICommand CancelCommand => new RelayCommand(() => {
            ContainerHandler.Container.Resolve<IDialogService>().DisposeLastDialog();
        });
    }
}
