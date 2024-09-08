using System;
using Notifications.Wpf.Core;
using Notifications.Wpf.Core.Controls;

namespace EyeClinic.WPF.AppServices.NotificationService
{
    public class NotificationService : INotificationService
    {
        private INotificationManager _notifier;

        public INotificationManager GetNotifier() {
            return _notifier ??= new NotificationManager(NotificationPosition.TopRight);
        }

        public void Error(string message) {
            GetNotifier().ShowAsync(new NotificationContent {
                Title = "Error",
                Message = message,
                Type = NotificationType.Error
            }, expirationTime: TimeSpan.FromSeconds(8));
        }

        public void Success(string message) {
            GetNotifier().ShowAsync(new NotificationContent {
                Title = "Success",
                Message = message,
                Type = NotificationType.Success
            }, expirationTime: TimeSpan.FromSeconds(1));
        }

        public void Warning(string message) {
            GetNotifier().ShowAsync(new NotificationContent {
                Title = "Warning",
                Message = message,
                Type = NotificationType.Warning
            }, expirationTime: TimeSpan.FromSeconds(6));
        }
        public void Information(string message) {
            GetNotifier().ShowAsync(new NotificationContent {
                Title = "Information",
                Message = message,
                Type = NotificationType.Information
            }, expirationTime: TimeSpan.FromSeconds(4));
        }
    }
}
