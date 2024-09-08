using System;
using System.Reflection;
using EyeClinic.WPF.AppServices.NotificationService;
using log4net;
using Unity;

namespace EyeClinic.WPF.Base
{
    public class ErrorHandlerBase : Bindable
    {
        private static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);


        [Dependency]
        private INotificationService NotificationService { get; set; }

        public void LogError(string message, Exception exception) {
            NotificationService?.Error(message);
            Log.Error(message, exception);
        }
    }
}
