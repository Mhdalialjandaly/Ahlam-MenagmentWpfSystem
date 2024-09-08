using System.Threading.Tasks;
using EyeClinic.Core.Interface;

namespace EyeClinic.DataAccess.IRepositories
{
    public interface IAppLanguageRepository : IInjectable
    {
        public Task<string> GetCurrentCulture(string deviceName);
        public Task SaveCulture(string languageCode, string deviceName);
        Task SetDevicePrinter(string printerName, string deviceName);
        Task<string> GetDevicePrinter(string deviceName);
        Task RequestNextPatient(string deviceName);
        Task<bool> IsWaitingForNextPatient();
        Task AcceptNext();
    }
}
