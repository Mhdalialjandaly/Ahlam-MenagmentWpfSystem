using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using EyeClinic.Core.Common;
using EyeClinic.Core.Enums;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base.Extends;
using EyeClinic.WPF.Base.Interfaces;
using Unity;
using WPFLocalizeExtension.Engine;

namespace EyeClinic.WPF.Base
{
    public class BaseAsyncViewModel : ErrorHandlerBase
    {
        public bool IsBusy { get; set; }
        public string BusyStatus { get; set; }

        public async void BusyExecute(Func<Task> action, string busyStatus = "Loading ..." ) {
            if (IsBusy)
                return;
            try {
                BusyStatus = busyStatus;
                IsBusy = true;
                await action();
                IsBusy = false;
            } catch (Exception ex) {
                LogError(ex.Message, ex);
                ContainerHandler.Container.Resolve<INotificationService>()
                    .Error(ex.GetBaseException().Message);
            } finally {
                IsBusy = false;
            }
        }
    }

    public class BaseViewModel : BaseAsyncViewModel
    {
        public FlowDirection Direction =>
            LocalizeDictionary.Instance.Culture.Name == "en-GB"
                ? FlowDirection.RightToLeft
                : FlowDirection.LeftToRight;

        public Operation Operation { get; set; }

        public bool Initialized { get; set; }

        public virtual Task Initialize() {
            return Task.FromResult(true);
        }

        public void SetLanguage(Language language) {
            LocalizeDictionary.Instance.SetCurrentThreadCulture = true;
            LocalizeDictionary.Instance.Culture = new CultureInfo(language.Code);
        }
    }

    public class BaseViewModel<TView> : BaseViewModel, IPresentable
        where TView : new()
    {
        protected TView View;
        public UIElement GetView() {
            if (View == null) {
                View = new TView();
                if (View is FrameworkElement element)
                    element.DataContext = this;
            }
            return View as UIElement;
        }

        protected bool IsDesignMode =>
            DesignerProperties.GetIsInDesignMode(new DependencyObject());
    }
}
