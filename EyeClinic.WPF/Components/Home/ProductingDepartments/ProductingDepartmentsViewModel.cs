using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using Notifications.Wpf.Core;

namespace EyeClinic.WPF.Components.Home.ProductingDepartments
{
    public partial class ProductingDepartmentsViewModel
    {
        public ProductingDepartmentsViewModel()
        {
               
        }
    }
    public partial class ProductingDepartmentsViewModel : BaseViewModel<ProductingDepartmentsView>
    {
        private readonly INotificationService _notificationService;
        public ProductingDepartmentsViewModel(INotificationService notificationService)
        {
                _notificationService = notificationService;
        }
    }
}
