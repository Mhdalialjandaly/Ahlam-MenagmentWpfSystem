using System.Threading.Tasks;

namespace EyeClinic.WPF.AppServices.NotificationService
{
    public interface INotificationService
    {
        void Error(string message);
        void Success(string message);
        void Warning(string message);
        void Information(string message);
    }
}
