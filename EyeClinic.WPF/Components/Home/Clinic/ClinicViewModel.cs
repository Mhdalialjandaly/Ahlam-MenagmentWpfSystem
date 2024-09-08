using System.Threading.Tasks;
using System.Windows.Controls;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Home.Reception.Queue;
using Unity;

namespace EyeClinic.WPF.Components.Home.Clinic
{
    public partial class ClinicViewModel
    {
        public ClinicViewModel() {
            if (IsDesignMode) { }
        }
    }

    public partial class ClinicViewModel : BaseViewModel<ClinicView>
    {
        private readonly IUnityContainer _container;

        public ClinicViewModel(IUnityContainer container) {
            _container = container;
        }
        public override async Task Initialize() {
            var queueModule = _container.Resolve<QueueViewModel>();
            await queueModule.Initialize();
            Content = queueModule.GetView() as QueueView;
        }

        public UserControl Content { get; set; }
    }
}
